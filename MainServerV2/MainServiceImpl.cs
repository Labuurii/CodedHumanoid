using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Core;
using MainServerV2;
using ServerClientSharedV2;

namespace ArenaHost
{
    internal class MainServiceImpl : MainServiceV2.MainServiceV2Base, IConfigurable
    {
        Config config;
        IDB db;
        PlayerAuth auth;
        SingleThreadScheduler in_memory_worker;
        VersionResult cached_version_result;
        IMatchMaker match_maker;

        internal MainServiceImpl(PlayerAuth auth, IDB db, SingleThreadScheduler in_memory_worker, IMatchMaker match_maker)
        {
            this.db = db;
            this.auth = auth;
            this.in_memory_worker = in_memory_worker;
            this.match_maker = match_maker;
        }

        public override async Task Subscribe(SubscriptionAttempt request, IServerStreamWriter<Event> responseStream, ServerCallContext context)
        {
            var login_result_ev = new Event
            {
                LoginResult = new Event_LoginResult()
            };
            var login_result = login_result_ev.LoginResult;

            Player player;
            List<BasicPlayerInfo> incoming_friend_requests;
            try
            {
                if(auth.Count > config.MaxPlayersOnline)
                {
                    login_result.Success = false;
                    await responseStream.WriteAsync(login_result_ev);
                    return;
                }

                var login_result_data = await auth.Login(request.Username, request.Password);
                if (!login_result_data.HasValue)
                {
                    login_result.Success = false;
                    await responseStream.WriteAsync(login_result_ev);
                    return;
                }
                player = login_result_data.Value.player;
                incoming_friend_requests = login_result_data.Value.pending_friend_requests_incoming;

            } catch(InvalidOperationException)
            {
                //Expected to happen when response stream is closed
                return;
            } catch(RpcException)
            {
                //Expected to happen when user logs out or gets disconnected for any other reason
                return;
            } catch(Exception e)
            {
                Log.Exception(e);
                return;
            }

            try
            {
                if (!player.event_stream.SetStream(responseStream, context))
                    return;

                login_result.Success = true;
                login_result.Token = ByteString.CopyFrom(player.session_id.ToByteArray());
                await in_memory_worker.Schedule(() =>
                {
                    login_result.Friends.AddRange(player.friends);
                    login_result.Ignored.AddRange(player.ignored);
                    login_result.PendingFriendRequestsOutgoing.AddRange(player.pending_friend_requests_outgoing);
                    login_result.PendingFriendRequestsIncoming.AddRange(incoming_friend_requests);
                });

                await responseStream.WriteAsync(login_result_ev);

                for(; ; )
                {
                    if (!player.online || !await player.event_stream.SendCurrentEvents())
                        return;
                    await Task.Delay(500);
                }

            } catch(InvalidOperationException)
            {
                //Expected to happen when response stream is closed
                return;
            }
            catch (RpcException)
            {
                //Expected to happen when user logs out or gets disconnected for any other reason

                return;
            }
            catch (Exception e)
            {
                Log.Exception(e);
                await auth.Logout(player.session_id);
            } 
        }

        public override async Task<RemoveFriendResult> RemoveFriend(RemoveFriendAttempt request, ServerCallContext context)
        {
            var result = new RemoveFriendResult
            {
                NoLongerFriend = true
            };

            var friend_permanent_id = request.PlayerId;
            Player player;
            try
            {
                player = auth.GetPlayer(context);
                if (player == null)
                    return result;
            } catch(Exception e)
            {
                Log.Exception(e);
                return result;
            }

            try
            {
                return await in_memory_worker.Schedule(() =>
                {
                    result.NoLongerFriend = false;
                    var friends = player.friends;
                    for(var i = 0; i < friends.Count; ++i)
                    {
                        if(friends[i].PermanentId == friend_permanent_id)
                        {
                            friends.RemoveAt(i);
                            result.NoLongerFriend = true;
                            break;
                        }
                    }

                    return result;
                });
            } catch(Exception e)
            {
                Log.Exception(e);
                return result;
            }
        }

        public override async Task<AnswerFriendRequestResult> AnswerFriendRequest(AnswerFriendRequestAttempt request, ServerCallContext context)
        {
            var result = new AnswerFriendRequestResult
            {
                NowFriend = false
            };

            var from_permanent_id = request.From;
            
            Player player;
            try
            {
                player = auth.GetPlayer(context);
                if(player == null)
                {
                    return result;
                }

            } catch(Exception e)
            {
                Log.Exception(e);
                return result;
            }

            try
            {
                return await await in_memory_worker.Schedule(async () =>
                {
                    result.NowFriend = request.Accepted;
                    var get_player_result = await auth.AnswerFriendRequest(player, from_permanent_id, request.Accepted);
                    if (!get_player_result)
                    {
                        result.NowFriend = false;
                    }

                    return result;
                });
            } catch(Exception e)
            {
                Log.Exception(e);
                return await Task.FromResult(result);
            }
        }

        public override async Task<FriendRequestResult> SendFriendRequest(FriendRequestAttempt request, ServerCallContext context)
        {
            var result = new FriendRequestResult
            {
                IsNowPending = false,
            };

            Player player;
            try
            {
                player = auth.GetPlayer(context);
                if (player == null)
                    return await Task.FromResult(result);
            } catch(Exception e)
            {
                Log.Exception(e);
                return await Task.FromResult(result);
            }

            try
            {
                return await await in_memory_worker.Schedule(async () =>
                {
                    if (player.friends.Count > config.MaxFriendSize)
                        return result;

                    var pfr = player.pending_friend_requests_outgoing;
                    if (pfr.Count > config.MaxFriendRequestSize)
                        return result;

                    var target_player_permanent_id = request.PlayerId;
                    if (player.HasRequestedPlayerAlready(target_player_permanent_id))
                    {
                        result.IsNowPending = true;
                        return result;
                    }

                    //NOTE: If the target player is ignoring player then this friend request should not go through

                    var success = await auth.SendFriendRequest(player, target_player_permanent_id);
                    if (!success)
                        return result;
                    result.IsNowPending = true;                    
                    return result;
                });
            } catch(Exception e)
            {
                Log.Exception(e);
                return await Task.FromResult(result);
            }
        }

        public override Task<InviteToGroupResult> InviteToGroup(InviteToGroupAttempt request, ServerCallContext context)
        {
            var result = new InviteToGroupResult
            {
                InvitedToGroup = false
            };

            Player player;
            Player target;
            try
            {
                player = auth.GetPlayer(context);
                target = auth.GetPlayer(request.PlayerId);
                if(player == null || target == null || player == target)
                {
                    return Task.FromResult(result);
                }

            } catch(Exception e)
            {
                Log.Exception(e);
                return Task.FromResult(result);
            }

            return in_memory_worker.Schedule(() =>
            {
                try
                {
                    if (target.group != null)
                        return result;

                    if (target.IsIgnoring(player))
                        return result;

                    Group group;
                    if (player.group != null) //Invite to already created group
                    {
                        group = player.group;
                        if (group.TotalPossibleMembers() > config.MaxGroupSize)
                            return result;
                        group.invited_to_group.Add(target);
                    }
                    else
                    {
                        group = new Group(player, target);
                    }
                    target.group = group;
                    in_memory_worker.Schedule(async () => //Time out the invite
                    {
                        await Task.Delay(config.GroupInviteTimeoutSec * 1000);
                        if(target.group != null)
                        {
                            if (target.group.RemoveInvitedMember(target))
                            {
                                var timed_out_ev = new Event
                                {
                                    GroupInviteTimedOut = new Event_GroupInviteTimedOut()
                                };
                                target.event_stream.Enqueue(timed_out_ev);
                            }
                        }
                    });

                    var ev = new Event
                    {
                        GotGroupInvite = new Event_GotGroupInvite
                        {
                            FromPlayer = player.BasicPlayerInfo()
                        }
                    };
                    target.event_stream.Enqueue(ev);

                    return result;
                } catch(Exception e)
                {
                    Log.Exception(e);
                    return result;
                }
            });
        }

        public override Task<AnswerGroupInviteResult> AnswerGroupInvite(AnswerGroupInviteAttempt request, ServerCallContext context)
        {
            var result = new AnswerGroupInviteResult
            {
                InGroup = false
            };

            Player player;
            try
            {
                player = auth.GetPlayer(context);
                if (player == null)
                {
                    return Task.FromResult(result);
                }

            }
            catch (Exception e)
            {
                Log.Exception(e);
                return Task.FromResult(result);
            }

            return in_memory_worker.Schedule(() =>
            {
                try
                {
                    var group = player.group;
                    if (group == null)
                        return result;

                    if(!request.Accepted)
                    {
                        if (!group.RemoveInvitedMember(player))
                            return result;
                        return result;
                    } else
                    {
                        if (!group.UpdatePlayerToMember(player))
                            return result;
                        if(player.arena_state == ArenaState.Queued)
                            match_maker.RemovePlayer(player);
                        result.InGroup = true;
                        result.Leader = group.leader.BasicPlayerInfo();
                        foreach(var group_member in group.players)
                        {
                            if (group_member == player || group_member == group.leader)
                                continue;
                            result.PlayersInGroup.Add(group_member.BasicPlayerInfo());
                        }
                        return result;
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);
                    return result;
                }
            });
        }

        public override async Task<LeaveGroupResult> LeaveGroup(LeaveGroupAttempt request, ServerCallContext context)
        {
            var result = new LeaveGroupResult();

            Player player;
            try
            {
                player = auth.GetPlayer(context);
                if (player == null)
                    return result;
            } catch(Exception e)
            {
                Log.Exception(e);
                return result;
            }

            try
            {
                return await in_memory_worker.Schedule(() =>
                {
                    var group = player.group;
                    if(group != null)
                    {
                        group.RemoveQualifiedMember(player);
                        if(player.arena_state == ArenaState.Queued)
                            match_maker.RemovePlayer(player);
                    }

                    return result;
                });
            } catch(Exception e)
            {
                Log.Exception(e);
                return result;
            }
        }

        public override async Task<IgnorePlayerResult> IgnorePlayer(IgnorePlayerAttempt request, ServerCallContext context)
        {
            var result = new IgnorePlayerResult
            {
                IsIgnored = false
            };

            Player player;
            BasicPlayerInfo target;
            try
            {
                player = auth.GetPlayer(context);
                if(player == null)
                {
                    return result;
                }
                var target_permanent_id = request.PlayerId;
                target = await db.GetBasicInfoOfPlayer(target_permanent_id);
                if (target == null)
                    return result;
            } catch(Exception e)
            {
                Log.Exception(e);
                return result;
            }

            try
            {
                return await in_memory_worker.Schedule(() =>
                {
                    player.AddIgnoredPlayerIfNotAlreadyThere(target);
                    result.IsIgnored = true;
                    return result;
                });
            } catch(Exception e)
            {
                Log.Exception(e);
                return result;
            }
        }

        public override async Task<UnIgnorePlayerResult> UnIgnorePlayer(UnIgnorePlayerAttempt request, ServerCallContext context)
        {
            var result = new UnIgnorePlayerResult
            {
                IsIgnored = true
            };

            Player player;
            var target_permanent_id = request.PlayerId;
            try
            {
                player = auth.GetPlayer(context);
                if (player == null)
                {
                    return result;
                }
            }
            catch (Exception e)
            {
                Log.Exception(e);
                return result;
            }

            try
            {
                return await in_memory_worker.Schedule(() =>
                {
                    if (!player.RemoveIgnoredPlayer(target_permanent_id))
                        result.IsIgnored = false;
                    else
                        result.IsIgnored = true;
                    return result;
                });
            } catch(Exception e)
            {
                Log.Exception(e);
                return result;
            }
        }

        public override async Task<SendMessageResult> SendMessage(SendMessageAttempt request, ServerCallContext context)
        {
            var result = new SendMessageResult
            {
                SentMessage = false
            };

            if (request.Msg.Length > config.MaxMessageLength)
                return result;

            Player player;
            try
            {
                player = auth.GetPlayer(context);
                if (player == null)
                    return result;
            } catch(Exception e)
            {
                Log.Exception(e);
                return result;
            }

            try
            {
                return await in_memory_worker.Schedule(() =>
                {
                    switch(request.TargetCase)
                    {
                        case SendMessageAttempt.TargetOneofCase.FriendTarget:
                            {
                                var target = request.FriendTarget.PlayerId;
                                var target_player = auth.GetPlayer(target);
                                if (target_player == null)
                                    break;
                                if (!player.HasFriend(target_player))
                                    break;
                                if (target_player.IsIgnoring(player))
                                    break;
                                var ev = new Event
                                {
                                    ReceivedMessage = new Event_ReceivedMessage
                                    {
                                        FromPlayer = player.BasicPlayerInfo(),
                                        Msg = request.Msg,
                                        MsgType = MessageType.Friend
                                    }
                                };
                                target_player.event_stream.Enqueue(ev);
                            }
                            break;
                        case SendMessageAttempt.TargetOneofCase.GroupTarget:
                            {
                                var group = player.group;
                                if (group == null)
                                    break;
                                var ev = new Event
                                {
                                    ReceivedMessage = new Event_ReceivedMessage
                                    {
                                        FromPlayer = player.BasicPlayerInfo(),
                                        Msg = request.Msg,
                                        MsgType = MessageType.Group
                                    }
                                };
                                foreach(var group_player in group.players)
                                {
                                    if (group_player == player || group_player.IsIgnoring(player))
                                        continue;
                                    group_player.event_stream.Enqueue(ev);
                                }
                                result.SentMessage = true;
                            }
                            break;
                        case SendMessageAttempt.TargetOneofCase.None:
                            break;
                        default:
                            throw new Exception("Unhandled enum value " + request.TargetCase);
                    }

                    return result;
                });
            } catch(Exception e)
            {
                Log.Exception(e);
                return result;
            }
        }

        public override Task<VersionResult> Version(VersionAttempt request, ServerCallContext context)
        {
            return Task.FromResult(cached_version_result);
        }

        public override async Task<QueueResult> Queue(QueueAttempt request, ServerCallContext context)
        {
            var q_result = new QueueResult
            {
                Queued = false
            };

            Player player;
            try
            {
                player = auth.GetPlayer(context);
                if (player == null)
                    return q_result;
            } catch(Exception e)
            {
                Log.Exception(e);
                return q_result;
            }

            try
            {
                return await in_memory_worker.Schedule(() =>
                {
                    if(player.arena_state != ArenaState.NotQueued)
                    {
                        q_result.Queued = player.arena_state == ArenaState.Queued ? true : false;
                        return q_result;
                    }

                    var group = player.group;
                    if(group != null)
                    {
                        if(group.leader != player)
                        {
                            q_result.Queued = false;
                            return q_result;
                        }

                        match_maker.AddPlayers(group.players);
                        var ev = new Event
                        {
                            GroupQueueStateChanged = new Event_GroupQueueStateChanged
                            {
                                Queued = true
                                //Matchmode is currently unused
                            }
                        };
                        foreach(var group_player in group.players)
                        {
                            if (group_player == player)
                                continue;
                            group_player.event_stream.Enqueue(ev);
                        }
                    } else
                    {
                        match_maker.AddPlayer(player);
                    }

                    q_result.Queued = true;
                    return q_result;
                });
            } catch(Exception e)
            {
                Log.Exception(e);
                return q_result;
            }
        }

        public override async Task<LeaveQueueResult> LeaveQueue(LeaveQueueAttempt request, ServerCallContext context)
        {
            var q_result = new LeaveQueueResult();

            Player player;
            try
            {
                player = auth.GetPlayer(context);
                if (player == null)
                {
                    q_result.NotInQueue = true;
                    return q_result;
                }
            }
            catch (Exception e)
            {
                Log.Exception(e);
                q_result.NotInQueue = true;
                return q_result;
            }

            try
            {
                return await in_memory_worker.Schedule(() =>
                {
                    if (player.arena_state != ArenaState.Queued)
                    {
                        q_result.NotInQueue = true;
                        return q_result;
                    }

                    var group = player.group;
                    if (group != null)
                    {
                        if (group.leader != player)
                        {
                            q_result.NotInQueue = false;
                            return q_result;
                        }

                        var ev = new Event
                        {
                            GroupQueueStateChanged = new Event_GroupQueueStateChanged
                            {
                                Queued = false
                            }
                        };
                        foreach (var group_player in group.players)
                            if (group_player != player)
                                group_player.event_stream.Enqueue(ev);

                    } else
                    {
                        match_maker.RemovePlayer(player);
                    }

                    
                    q_result.NotInQueue = true;
                    return q_result;
                });
            }
            catch (Exception e)
            {
                Log.Exception(e);
                return q_result;
            }
        }

        public void SetConfig(Config config)
        {
            this.config = config;
            cached_version_result = new VersionResult
            {
                SemverId = config.GameVersion
            };
        }
    }
}
