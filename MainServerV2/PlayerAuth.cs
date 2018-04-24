using Google.Protobuf;
using Grpc.Core;
using MainServerV2;
using ServerClientSharedV2;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaHost
{
    internal class PlayerAuth : IDisposable
    {
        

        IDB db;
        SingleThreadScheduler in_memory_worker;
        ConcurrentDictionary<Guid, Player> session_mapped_players = new ConcurrentDictionary<Guid, Player>();
        ConcurrentDictionary<long, Player> permanent_id_mapped_players = new ConcurrentDictionary<long, Player>();

        internal PlayerAuth(IDB db, SingleThreadScheduler in_memory_worker)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
            this.in_memory_worker = in_memory_worker ?? throw new ArgumentNullException(nameof(in_memory_worker));
        }

        internal struct LoginResult
        {
            internal Player player;
            internal List<BasicPlayerInfo> pending_friend_requests_incoming;
        }

        internal int Count
        {
            get
            {
                return permanent_id_mapped_players.Count;
            }
        }

        internal async Task<LoginResult?> Login(string username, string password)
        {
            if (disposedValue)
            {
                Log.Warning("Tried to log in player while player auth is disposing.");
                return null;
            }

            if (string.IsNullOrWhiteSpace(username)
                || string.IsNullOrWhiteSpace(password))
                return null;

            var info = await db.GetPlayerInfoAndPassword(username);
            if (!info.HasValue)
                return null;
            
            if (!BCrypt.Net.BCrypt.Verify(password, info.Value.password_hash))
                return null;

            var session_id = Guid.NewGuid();
            var player = new Player(session_id, info.Value.player_info);
            if (!permanent_id_mapped_players.TryAdd(info.Value.player_info.permanent_id, player))
                return null;

            session_mapped_players.TryAdd(session_id, player);
            player.online = true;
            return new LoginResult
            {
                player = player,
                pending_friend_requests_incoming = info.Value.player_info.pending_friend_requests_incoming
            };
        }

        internal Player GetPlayer(ServerCallContext context)
        {
            if (disposedValue)
            {
                Log.Warning("Tried to get player while player auth is disposing.");
                return null;
            }

            var header = GetHeader(context, Constants.SessionTokenHeader);
            Guid session_token;
            if (header == null || !GuidOps.TryParse(header, out session_token))
                return null;
            Player player;
            session_mapped_players.TryGetValue(session_token, out player);
            return player;
        }

        internal Player GetPlayer(long permanent_id)
        {
            if(disposedValue)
            {
                Log.Warning("Tried to get player while player auth is disposing.");
                return null;
            }

            Player player;
            permanent_id_mapped_players.TryGetValue(permanent_id, out player);
            return player;
        }

        internal async Task Logout(ServerCallContext context)
        {
            if (disposedValue)
            {
                Log.Warning("Tried to log out player while auth is disposing.");
                return;
            }

            var header = GetHeader(context, Constants.SessionTokenHeader);
            Guid session_id;
            if (!GuidOps.TryParse(header, out session_id))
                return;
            await Logout(session_id);
        }

        internal async Task Logout(Guid session_id)
        {
            if (disposedValue)
            {
                Log.Warning("Tried to log out player while player auth is disposing.");
                return;
            }

            Player player;
            if (!session_mapped_players.TryRemove(session_id, out player))
                return;
            if (player == null)
                return;
            await logout_impl(player);
        }

        private async Task logout_impl(Player player)
        {
            player.online = false;
            if (!permanent_id_mapped_players.TryRemove(player.permanent_id, out player))
            {
                Log.Warning("When logging out player it is expected that permanent_id_mapped_players contains the player when the player is removed from session_mapped_players.");
                return;
            }

            await in_memory_worker.Schedule(async () =>
            {
                var friends = player.friends;
                if (friends == null || friends.Count == 0)
                    return;
                Event ev = null;

                foreach (var friend_info in friends)
                {
                    var permanent_id = friend_info.PermanentId;
                    Player friend;
                    if (permanent_id_mapped_players.TryGetValue(permanent_id, out friend) && friend != null)
                    {
                        if (ev == null)
                            ev = player.EventOnlineStateChanged(OnlineState.Offline);
                        friend.event_stream.Enqueue(ev);
                    }
                }

                var group = player.group;
                if (group != null)
                {
                    group.RemoveInvitedMember(player);
                }

                var arena = player.arena;
                if(arena != null)
                {
                    arena.RemovePlayerAsync(player);
                }

                var player_info = player.ToPlayerInfo();
                await Task.Run(async () => await db.SavePlayerInfo(player_info));
            });
        }

        internal struct AnswerRequestResult
        {
            internal BasicPlayerInfo basic_player_info;
            internal Player player;
        }

        internal Task<bool> SendFriendRequest(Player from, long to_permanent_id)
        {
            if(disposedValue)
            {
                Log.Warning("Tried to send friend request while player auth is disposing.");
                return null;
            }

            return Task.Run(async () =>
            {
                if (!await db.AddPendingFriendRequest(from.permanent_id, to_permanent_id))
                    return false;

                Player to_player;
                if (permanent_id_mapped_players.TryGetValue(to_permanent_id, out to_player))
                {
                    
                    var ev = new Event
                    {
                        FriendRequest = new Event_FriendRequest
                        {
                            BasicPlayerInfo = from.BasicPlayerInfo()
                        }
                    };
                    to_player.event_stream.Enqueue(ev);
                    
                }

                return true;
            });
        }

        /// <summary>
        /// This one can be called from the in memory worker thread.
        /// Internally it creates a task on the default scheduler.
        /// After this the scheduler should jump back.
        /// </summary>
        /// <param name="answerer_permanent_id"></param>
        /// <returns></returns>
        internal Task<bool> AnswerFriendRequest(Player answerer, long from_permanent_id, bool accepted)
        {
            if (disposedValue)
            {
                Log.Warning("Tried to answer friend request while player auth is disposing.");
                return Task.FromResult(false);
            }

            return Task.Run(async () =>
            {
                if (!await db.AnswerFriendRequest(answerer.permanent_id, from_permanent_id, accepted))
                    return false;

                Player player;
                if (permanent_id_mapped_players.TryGetValue(answerer.permanent_id, out player)) //TODO: Am I sending the event here?
                {
                    var ev = new Event
                    {
                        FriendAcceptedRequest = new Event_FriendAcceptedRequest
                        {
                            PlayerInfo = player.BasicPlayerInfo()
                        }
                    };
                    player.event_stream.Enqueue(ev);

                    in_memory_worker.Schedule(() => BasicPlayerInfoOps.RemoveOne(player.pending_friend_requests_outgoing, answerer.BasicPlayerInfo()));
                }

                return true;
            });
        }

        private static byte[] GetHeader(ServerCallContext context, string header_name)
        {
            foreach(var header in context.RequestHeaders)
            {
                if (header.Key == header_name)
                    return header.ValueBytes;
            }
            return null;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                disposedValue = true;
                if (disposing)
                {
                    Log.Msg("Shutting down player auth...");
                    foreach(var kv in session_mapped_players)
                    {
                        try
                        {
                            Task.Run(async () => await logout_impl(kv.Value)).Wait();
                        } catch(Exception e)
                        {
                            Log.Exception(e);
                        }
                    }
                    session_mapped_players.Clear();
                    permanent_id_mapped_players.Clear();
                    Log.Msg("Done shutting down player auth.");
                }
            }
        }

        ~PlayerAuth() {
            Debug.Assert(disposedValue);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
