using Google.Protobuf;
using MainServerV2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaHost
{
    internal struct PlayerInfo
    {
        internal long permanent_id;
        internal string display_name;
        internal List<BasicPlayerInfo> friends, ignored, pending_friend_requests_incoming, pending_friend_requests_outgoing;
    }

    internal struct PlayerPublicInfo
    {
        internal long permanent_id;
        internal string display_name;

        internal BasicPlayerInfo ToBasicPlayerInfo()
        {
            return new BasicPlayerInfo
            {
                DisplayName = display_name,
                PermanentId = permanent_id
            };
        }
    }

    internal class Player
    {
        readonly BasicPlayerInfo cached_basic_player_info;

        internal volatile bool online;
        internal Group group;

        internal readonly long permanent_id;
        internal readonly Guid session_id;

        /// <summary>
        /// Can be null.
        /// </summary>
        internal readonly string display_name;

        internal readonly List<BasicPlayerInfo> friends;
        internal readonly List<BasicPlayerInfo> ignored;
        internal readonly List<BasicPlayerInfo> pending_friend_requests_outgoing;

        internal readonly MTNetOutStream<Event> event_stream = MTNetOutStream<Event>.Create();

        internal volatile ArenaState arena_state;
        internal volatile SingleArena arena;

        internal Player(Guid session_id, PlayerInfo pi)
        {
            permanent_id = pi.permanent_id;
            display_name = pi.display_name;
            friends = pi.friends;
            ignored = pi.ignored;
            pending_friend_requests_outgoing = pi.pending_friend_requests_outgoing;
            cached_basic_player_info = new BasicPlayerInfo
            {
                DisplayName = display_name,
                PermanentId = pi.permanent_id,
                Online = online
            };
        }

        internal Event EventOnlineStateChanged(OnlineState state)
        {
            return new Event
            {
                FriendOnlineStateChanged = new Event_FriendOnlineStateChanged
                {
                    PlayerId = permanent_id,
                    OnlineState = state
                }
            };
        }

        internal void RemoveFromArena()
        {
            arena = null;
            arena_state = ArenaState.NotQueued;
        }

        internal List<Event_FriendRequest> NetworkPendingFriendRequests()
        {
            var result = new List<Event_FriendRequest>(pending_friend_requests_outgoing.Count);
            for(var i = 0; i < pending_friend_requests_outgoing.Count; ++i)
            {
                var pfr = pending_friend_requests_outgoing[i];
                result.Add(new Event_FriendRequest
                {
                    BasicPlayerInfo = pfr
                });
            }

            return result;
        }

        internal PlayerInfo ToPlayerInfo()
        {
            return new PlayerInfo
            {
                permanent_id = permanent_id,
                display_name = display_name,
                friends = new List<BasicPlayerInfo>(friends),
                ignored = new List<BasicPlayerInfo>(ignored),
            };
        }

        public override bool Equals(object obj)
        {
            {
                var other = obj as Player;
                if (other != null)
                    return permanent_id == other.permanent_id;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return permanent_id.GetHashCode();
        }

        internal BasicPlayerInfo BasicPlayerInfo()
        {
            cached_basic_player_info.Online = online;
            return cached_basic_player_info;
        }

        internal bool HasRequestedPlayerAlready(long permanent_id)
        {
            for(var i = 0; i < pending_friend_requests_outgoing.Count; ++i)
            {
                if (pending_friend_requests_outgoing[i].PermanentId == permanent_id)
                    return true;
            }

            return false;
        }

        internal void AddIgnoredPlayerIfNotAlreadyThere(BasicPlayerInfo player)
        {
            for(var i = 0; i < ignored.Count; ++i)
            {
                var info = ignored[i];
                if(info == null)
                {
                    Log.Warning("Ignored list contains nullable values.");
                    continue;
                }

                if(info.PermanentId == player.PermanentId)
                {
                    //There is already an entry of this player.
                    return;
                }
            }

            ignored.Add(player);
        }

        internal bool RemoveIgnoredPlayer(long target_permanent_id)
        {
            for(var i = 0; i < ignored.Count; ++i)
            {
                var info = ignored[i];
                if(info == null)
                {
                    Log.Warning("Ignored list contains nullable values.");
                    continue;
                }

                if(info.PermanentId == target_permanent_id)
                {
                    ignored.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        internal bool HasFriend(Player target_player)
        {
            var target_permanent_id = target_player.cached_basic_player_info.PermanentId;
            
            for(var i = 0; i < friends.Count; ++i)
            {
                var info = friends[i];
                if (info.PermanentId == target_permanent_id)
                    return true;
            }

            return false;
        }

        internal bool RemoveFriendForcefully(Player player)
        {
            var player_basic_info = player.BasicPlayerInfo();
            for(var i = 0; i < friends.Count; ++i)
            {
                if(friends[i].PermanentId == player_basic_info.PermanentId)
                {
                    friends.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        internal bool IsIgnoring(Player player)
        {
            var other_perm_id = player.permanent_id;
            for(var i = 0; i < ignored.Count; ++i)
            {
                if (ignored[i].PermanentId == other_perm_id)
                    return true;
            }

            return false;
        }
    }
}
