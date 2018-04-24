using MainServerV2;
using MySql.Data.MySqlClient;
using ServerClientSharedV2;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaHost
{
    internal class DB : IDB, IDisposable
    {
        string connection_str;

        //GetPlayerInfoAndPassword
        const string SelectPlayerPersonalInfo = "SELECT gamers.id, local_logins.password_hash, gamers.display_name FROM local_logins INNER JOIN gamers ON local_logins.gamer_id = gamers.id WHERE username = @username LIMIT 1";
        const string SelectFriendsOfPlayer = "SELECT a,b FROM friends WHERE a = @a OR b = @b";
        const string SelectIgnoredOfPlayer = "SELECT ignored FROM ignored WHERE ignorer = @ignorer";
        const string SelectPendingFriendRequests = "SELECT to_, from_ FROM friend_requests WHERE to_ = @gamer OR from_ = @gamer";

        //SavePlayerInfo
        const string DeleteFriendsOfPlayer = "DELETE FROM friends WHERE a = @permanent_id OR b = @permanent_id";
        const string DeleteIgnoredOfPlayer = "DELETE FROM ignored WHERE ignorer = @permanent_id";
        const string DeleteFriendRequestsOfPlayer = "DELETE FROM friend_requests WHERE from_ = @permanent_id";
        const string InsertFriendsOfPlayerProlog = "INSERT INTO friends(a, b) VALUES";
        const string InsertIgnoredOfPlayerProlog = "INSERT INTO ignored(ignorer, ignored) VALUES";
        const string InsertFriendRequestsOfPlayerProlog = "INSERT INTO friend_requests(from_, to_) VALUES";

        //AddPendingFriendRequest
        const string InsertFriendRequest = "INSERT INTO friend_requests(from_, to_) VALUES(@from, @to)";

        //GetBasicPlayerInfoOfPlayer
        const string SelectDisplayName = "SELECT display_name FROM gamers WHERE id = @permanent_id LIMIT 1";

        //GetBasicInfoOfPlayerAndAnswerFriendRequest
        const string DeleteFriendRequest = "DELETE FROM friend_requests WHERE from_ = @from AND to_ = @to";

        public DB(string connection_str)
        {
            this.connection_str = connection_str ?? throw new ArgumentNullException(nameof(connection_str));
        }

        private MySqlConnection get_conn()
        {
            return new MySqlConnection(connection_str);
        }

        public async Task<PlayerPasswordAndInfo?> GetPlayerInfoAndPassword(string username)
        {
            try
            {
                var player_info = new PlayerPasswordAndInfo
                {
                    player_info = new PlayerInfo
                    {
                        friends = new List<BasicPlayerInfo>(),
                        ignored = new List<BasicPlayerInfo>(),
                        pending_friend_requests_incoming = new List<BasicPlayerInfo>(),
                        pending_friend_requests_outgoing = new List<BasicPlayerInfo>()
                    }
                };

                using (var conn = get_conn())
                {
                    using (var personal_info_cmd = new MySqlCommand
                    {
                        CommandText = SelectPlayerPersonalInfo,
                        Connection = conn
                    })
                    {
                        personal_info_cmd.Parameters.AddWithValue("@username", username);
                        var personal_info_reader = await personal_info_cmd.ExecuteReaderAsync();
                        if (!await personal_info_reader.ReadAsync())
                            return null;

                        player_info.player_info.permanent_id = personal_info_reader.GetInt64(0);
                        player_info.password_hash = personal_info_reader.GetString(1);
                        player_info.player_info.display_name = personal_info_reader.GetString(2);
                    }

                    using (var friends_cmd = new MySqlCommand
                    {
                        CommandText = SelectFriendsOfPlayer,
                        Connection = conn
                    })
                    {
                        friends_cmd.Parameters.AddWithValue("@a", player_info.player_info.permanent_id);
                        friends_cmd.Parameters.AddWithValue("@b", player_info.player_info.permanent_id);
                        var friends_task = friends_cmd.ExecuteReaderAsync();

                        using (var ignored_conn = get_conn())
                        using (var ignored_cmd = new MySqlCommand
                        {
                            CommandText = SelectIgnoredOfPlayer,
                            Connection = ignored_conn
                        })
                        {
                            ignored_cmd.Parameters.AddWithValue("@ignorer", player_info.player_info.permanent_id);
                            var ignored_task = ignored_cmd.ExecuteReaderAsync();

                            using(var pending_conn = get_conn())
                            using (var pending_cmd = new MySqlCommand
                            {
                                CommandText = SelectPendingFriendRequests,
                                Connection = pending_conn
                            })
                            {
                                pending_cmd.Parameters.AddWithValue("@gamer", player_info.player_info.permanent_id);
                                var pending_task = pending_cmd.ExecuteReaderAsync();
                                var tasks = new List<Task<DbDataReader>>(new[] { friends_task, ignored_task, pending_task });

                                for(int i = 0; i < tasks.Count;)
                                {
                                    var completed_task = await Task.WhenAny(tasks);
                                    var reader = await completed_task;
                                    var idx = tasks.IndexOf(completed_task);
                                    tasks[idx] = null;

                                    switch(idx)
                                    {
                                        case 0:
                                            {
                                                while(await reader.ReadAsync())
                                                {
                                                    var a = reader.GetInt64(0);
                                                    var b = reader.GetInt64(1);

                                                    if(a == player_info.player_info.permanent_id)
                                                    {
                                                        if(b == player_info.player_info.permanent_id)
                                                        {
                                                            Log.Warning("Database contains friend matchings which match with itself.");
                                                            continue;
                                                        }

                                                        var friend = await GetBasicInfoOfPlayer(b);
                                                        if (friend == null)
                                                            continue;
                                                        player_info.player_info.friends.Add(friend);

                                                    } else if(b == player_info.player_info.permanent_id)
                                                    {
                                                        if(a == player_info.player_info.permanent_id)
                                                        {
                                                            Log.Warning("Database contains friend matchings which match with itself.");
                                                            continue;
                                                        }

                                                        var friend = await GetBasicInfoOfPlayer(a);
                                                        if (friend == null)
                                                            continue;
                                                        player_info.player_info.friends.Add(friend);

                                                    } else
                                                    {
                                                        Log.Fatal("Expected query to give the actual player in either a or b");
                                                    }
                                                }
                                            }
                                            break;
                                        case 1:
                                            {
                                                while(await reader.ReadAsync())
                                                {
                                                    var ignored_permanent_id = reader.GetInt64(0);

                                                    var ignored = await GetBasicInfoOfPlayer(ignored_permanent_id);
                                                    if (ignored == null)
                                                        continue;
                                                    player_info.player_info.ignored.Add(ignored);
                                                }
                                            }
                                            break;
                                        case 2:
                                            {
                                                while(await reader.ReadAsync())
                                                {
                                                    var to = reader.GetInt64(0);
                                                    var from = reader.GetInt64(1);

                                                    if (from == player_info.player_info.permanent_id) //Outgoing
                                                    {
                                                        if (to == player_info.player_info.permanent_id)
                                                        {
                                                            Log.Warning("Database contains friend matchings which match with itself.");
                                                            continue;
                                                        }

                                                        var other_player = await GetBasicInfoOfPlayer(to);
                                                        if (other_player == null)
                                                            continue;
                                                        player_info.player_info.pending_friend_requests_outgoing.Add(other_player);
                                                    }
                                                    else if (to == player_info.player_info.permanent_id) //Incoming
                                                    {
                                                        if (from == player_info.player_info.permanent_id)
                                                        {
                                                            Log.Warning("Database contains friend matchings which match with itself.");
                                                            continue;
                                                        }

                                                        var other_player = await GetBasicInfoOfPlayer(to);
                                                        if (other_player == null)
                                                            continue;
                                                        player_info.player_info.pending_friend_requests_incoming.Add(other_player);

                                                    }
                                                    else
                                                    {
                                                        Log.Fatal("Expected query to give the actual player in either from or to");
                                                    }
                                                }
                                            }
                                            break;
                                        default:
                                            throw new Exception("Expects there to be three tasks.");
                                    }
                                }
                                
                            }
                        }
                    }
                }

                return player_info;
            } catch(Exception e)
            {
                Log.Exception(e);
                return null;
            }
        }

        public async Task SavePlayerInfo(PlayerInfo player_info)
        {
            try
            {
                using (var conn = get_conn())
                {
                    var transaction = await conn.BeginTransactionAsync();
                    try
                    {
                        using (var cmd = new MySqlCommand
                        {
                            CommandText = DeleteFriendsOfPlayer,
                            Connection = conn
                        })
                        {
                            cmd.Parameters.AddWithValue("@permanent_id", player_info.permanent_id);
                            await cmd.ExecuteNonQueryAsync();
                        }

                        using (var cmd = new MySqlCommand
                        {
                            CommandText = DeleteIgnoredOfPlayer,
                            Connection = conn
                        })
                        {
                            cmd.Parameters.AddWithValue("@permanent_id", player_info.permanent_id);
                            await cmd.ExecuteNonQueryAsync();
                        }

                        using (var cmd = new MySqlCommand
                        {
                            CommandText = DeleteFriendRequestsOfPlayer,
                            Connection = conn
                        })
                        {
                            cmd.Parameters.AddWithValue("@permanent_id", player_info.permanent_id);
                            await cmd.ExecuteNonQueryAsync();
                        }

                        using (var cmd = new MySqlCommand
                        {
                            Connection = conn
                        })
                        {
                            var builder = TupleListBuilder.Create(InsertFriendsOfPlayerProlog);
                            for(var i = 0; i < player_info.friends.Count; ++i)
                            {
                                var friend = player_info.friends[i];
                                builder.StartTuple();
                                builder.Value(player_info.permanent_id);
                                builder.Join();
                                var friend_permanent_id = friend.PermanentId;
                                builder.Value(friend_permanent_id);
                                builder.EndTuple();

                                if(i != player_info.friends.Count - 1)
                                    builder.Join();
                            }

                            cmd.CommandText = builder.ToString();

                            await cmd.ExecuteNonQueryAsync();
                        }

                        using (var cmd = new MySqlCommand
                        {
                            Connection = conn
                        })
                        {
                            var builder = TupleListBuilder.Create(InsertIgnoredOfPlayerProlog);
                            for (var i = 0; i < player_info.ignored.Count; ++i)
                            {
                                var ignored = player_info.ignored[i];
                                builder.StartTuple();
                                builder.Value(player_info.permanent_id);
                                builder.Join();
                                var ignored_permanent_id = ignored.PermanentId;
                                builder.Value(ignored_permanent_id);
                                builder.EndTuple();

                                if (i != player_info.friends.Count - 1)
                                    builder.Join();
                            }

                            cmd.CommandText = builder.ToString();

                            await cmd.ExecuteNonQueryAsync();
                        }

                        using (var cmd = new MySqlCommand
                        {
                            Connection = conn
                        })
                        {
                            var builder = TupleListBuilder.Create(InsertFriendRequestsOfPlayerProlog);
                            for (var i = 0; i < player_info.pending_friend_requests_outgoing.Count; ++i)
                            {
                                var friend_requested = player_info.pending_friend_requests_outgoing[i];
                                builder.StartTuple();
                                builder.Value(player_info.permanent_id);
                                builder.Join();
                                var friend_requested_permanent_id = friend_requested.PermanentId;

                                builder.Value(friend_requested_permanent_id);
                                builder.EndTuple();

                                if (i != player_info.friends.Count - 1)
                                    builder.Join();
                            }

                            cmd.CommandText = builder.ToString();

                            await cmd.ExecuteNonQueryAsync();
                        }

                        transaction.Commit();
                    } catch(Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            } catch(Exception e)
            {
                Log.Exception(e);
            }
        }

        public async Task<bool> AddPendingFriendRequest(long permanent_id, long target_player_permanent_id)
        {
            try
            {
                using(var conn = get_conn())
                {
                    using (var cmd = new MySqlCommand
                    {
                        CommandText = InsertFriendRequest,
                        Connection = conn
                    })
                    {
                        cmd.Parameters.AddWithValue("@from", permanent_id);
                        cmd.Parameters.AddWithValue("@to", target_player_permanent_id);

                        return await cmd.ExecuteNonQueryAsync() == 1;
                    }
                }
            } catch(Exception e)
            {
                Log.Exception(e);
                return false;
            }
        }

        public async Task<BasicPlayerInfo> GetBasicInfoOfPlayer(long permanent_id)
        {
            try
            {
                using (var conn = get_conn())
                {
                    using (var cmd = new MySqlCommand
                    {
                        CommandText = SelectDisplayName,
                        Connection = conn
                    })
                    {
                        cmd.Parameters.AddWithValue("@permanent_id", permanent_id);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (!await reader.ReadAsync())
                                return null;
                            var display_name = reader.GetString(0);
                            return new BasicPlayerInfo
                            {
                                PermanentId = permanent_id,
                                DisplayName = display_name,
                                Online = false
                            };
                        }
                    }
                }
            } catch(Exception e)
            {
                Log.Exception(e);
                return null;
            }
        }

        public async Task<BasicPlayerInfo> GetBasicInfoOfPlayerAndAnswerFriendRequest(long answerer_permanent_id, long from_permanent_id, bool accepted)
        {
            var basic_info = await GetBasicInfoOfPlayer(from_permanent_id);
            if (basic_info == null)
                return null;
            if (!await AnswerFriendRequest(answerer_permanent_id, from_permanent_id, accepted))
                return null;
            return basic_info;
        }

        public async Task<bool> AnswerFriendRequest(long answerer_permanent_id, long from_permanent_id, bool accepted)
        {
            try
            {
                using (var conn = get_conn())
                {
                    using (var cmd = new MySqlCommand
                    {
                        CommandText = DeleteFriendRequest,
                        Connection = conn
                    })
                    {
                        cmd.Parameters.AddWithValue("@from", from_permanent_id);
                        cmd.Parameters.AddWithValue("@to", answerer_permanent_id);
                        if (await cmd.ExecuteNonQueryAsync() == 1)
                            return true;
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Exception(e);
                return false;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    MySqlConnection.ClearAllPools();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion

        struct TupleListBuilder
        {
            StringBuilder sql;

            internal static TupleListBuilder Create(string prolog)
            {
                var r = new TupleListBuilder();
                r.sql = new StringBuilder(prolog.Length * 2);
                r.sql.Append(prolog);
                return r;
            }

            internal void StartTuple()
            {
                sql.Append('(');
            }

            internal void Value(object v)
            {
                sql.Append(v.ToString()); //TODO: Horrendous on the allocation side. Generics will help against that?
            }

            internal void Join()
            {
                sql.Append(',');
            }

            internal void EndTuple()
            {
                sql.Append(')');
            }

            public override string ToString()
            {
                return sql.ToString();
            }
        }
    }
}
