using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using MainServerV2;
using UnityEngine;

namespace Assets.Game.Scripts
{
    public class Group
    {
        public BasicPlayerInfo leader;
        public List<BasicPlayerInfo> normal_members = new List<BasicPlayerInfo>();
    }

    public static class MainServerClientV2DataStore
    {
        public static MainServerClientV2LL client;

        public static readonly List<BasicPlayerInfo> pending_friends_incoming = new List<BasicPlayerInfo>();
        public static readonly List<BasicPlayerInfo> pending_friends_outgoing = new List<BasicPlayerInfo>();
        public static readonly List<BasicPlayerInfo> friends = new List<BasicPlayerInfo>();
        public static readonly List<BasicPlayerInfo> ignored = new List<BasicPlayerInfo>();

        public static BasicPlayerInfo group_invite;
        public static Group group;

        public static void RegisterClient(MainServerClientV2LL client)
        {
            MainServerClientV2DataStore.client = client;

            //Rpcs
            client.OnSendFriendRequest += on_send_friend_request;
            client.OnAnswerFriendRequest += on_answer_friend_request;
            client.OnRemoveFriend += on_remove_friend;
            client.OnIgnorePlayer += on_ignore_player;
            client.OnUnIgnorePlayer += on_unignored_player;
            //client.InviteToGroup No useful data to capture 
            client.OnAnswerGroupInvite += on_answer_group_invite;
            client.OnLeaveGroup += on_leave_group;

            //Events
            client.OnLoginResult += on_login_result;
            client.OnFriendAcceptedRequest += on_friend_request;
            client.OnFriendAcceptedRequest += on_friend_accepted_request;
            client.OnFriendRemovedYou += on_friend_removed_you;
            client.OnFriendOnlineStateChanged += on_friend_online_state_changed;
            client.OnGroupInvite += on_group_invite;
            client.OnGroupInviteTimedOut += on_group_invite_timed_out;
            client.OnPlayerAnsweredGroupInvite += on_player_answered_group_invite;
            client.OnPlayerLeftGroup += on_player_left_group;
        }

        private static void on_player_left_group(Event_PlayerLeftGroup ev)
        {
            if(group == null)
            {
                Debug.LogError("Got player left group event from server but data store thinks we are not in a group.");
            }

            if(ev.NewLeader != null)
            {
                group.leader = ev.NewLeader;
            }

            group.normal_members.Clear();
            group.normal_members.AddRange(ev.CurrentPlayers);
        }

        private static void on_player_answered_group_invite(Event_PlayerAnsweredGroupInvite ev)
        {
            if (group == null)
            {
                Debug.LogError("Git player answered group invite message when data store thinks we are not in a group.");
                return; //Not recoverable
            }

            if (ev.Accepted)
            {
                group.normal_members.Add(ev.Player);
            }
        }

        private static void on_group_invite_timed_out(Event_GroupInviteTimedOut ev)
        {
            if(group_invite == null)
            {
                Debug.LogError("Got group invite timed out message but the data store thought there where no group invite in the first place!");
            }
            group_invite = null;
        }

        private static void on_group_invite(Event_GotGroupInvite ev)
        {
            group_invite = ev.FromPlayer;
        }

        private static void on_friend_online_state_changed(Event_FriendOnlineStateChanged ev)
        {
            var friend = BasicPlayerInfoOps.GetInList(friends, ev.PlayerId);
            if(friend == null)
            {
                Debug.LogError("Server told us about online state of player. But we do not know about the player and server does not send any player info here.");
                return;
            }
            friend.Online = ev.OnlineState == OnlineState.Online;
        }

        private static void on_friend_removed_you(Event_FriendRemovedYou ev)
        {
            if(!BasicPlayerInfoOps.RemoveOne(friends, ev.PlayerInfo))
            {
                Debug.Log("Server told us a friend has been removed but the data store did not know about the friend.");
            }
        }

        private static void on_friend_accepted_request(Event_FriendAcceptedRequest ev)
        {
            if(!BasicPlayerInfoOps.RemoveOne(pending_friends_outgoing, ev.PlayerInfo))
            {
                Debug.Log("Server told us a friend request has been accepted but the data store did not know about the outgoing friend request.");
            }

            friends.Add(ev.PlayerInfo);
        }

        private static void on_friend_request(Event_FriendAcceptedRequest ev)
        {
            if(BasicPlayerInfoOps.Contains(pending_friends_incoming, ev.PlayerInfo))
            {
                Debug.LogErrorFormat("Request from {0} is already pending.", ev.PlayerInfo.DisplayName);
                return;
            }
            pending_friends_incoming.Add(ev.PlayerInfo);
        }

        private static void on_login_result(Event_LoginResult ev)
        {
            pending_friends_incoming.Clear();
            pending_friends_outgoing.Clear();
            friends.Clear();
            ignored.Clear();

            if(ev.Success)
            {
                pending_friends_incoming.AddRange(ev.PendingFriendRequestsIncoming);
                pending_friends_outgoing.AddRange(ev.PendingFriendRequestsOutgoing);
                friends.AddRange(ev.Friends);
                ignored.AddRange(ev.Ignored);
            }
        }

        private static void on_leave_group(LeaveGroupResult r)
        {
            group = null;
        }

        private static void on_answer_group_invite(AnswerGroupInviteResult r)
        {
            if(r.InGroup)
            {
                if(group != null)
                {
                    Debug.LogError("Server told us we just answered a group invite and the group still exists. But according to the data store a group was formed before.");
                }
                if(r.Leader == null)
                {
                    Debug.LogError("The group leader is null!!");
                    return;
                }
                if(r.PlayersInGroup == null)
                {
                    Debug.LogError("There are no players in the group.");
                }

                group = new Group();
                group.leader = r.Leader;
                group.normal_members.AddRange(r.PlayersInGroup);
            }
        }

        private static void on_unignored_player(BasicPlayerInfo player, UnIgnorePlayerResult r)
        {
            if(!r.IsIgnored)
            {
                if(!BasicPlayerInfoOps.RemoveOne(ignored, player))
                {
                    Debug.LogError("Server said to us player is ignored but according to the data store the player was never ignored!");
                }
            }
        }

        private static void on_ignore_player(BasicPlayerInfo player, IgnorePlayerResult r)
        {
            if(r.IsIgnored)
            {
                if(BasicPlayerInfoOps.Contains(ignored, player))
                {
                    Debug.Log("Server said to us player is ignored and the data store already have that player as ignored. Hopefully we just tried to ignore an already ignored player.");
                    return;
                }

                ignored.Add(player);
            }
        }

        private static void on_remove_friend(BasicPlayerInfo player, RemoveFriendResult r)
        {
            if(r.NoLongerFriend)
            {
                if(!BasicPlayerInfoOps.RemoveOne(friends, player))
                {
                    Debug.Log("Tried to remove friend but friend was not well... a friend.");
                }
            }
        }

        private static void on_answer_friend_request(BasicPlayerInfo player, AnswerFriendRequestResult r)
        {
            if(r.NowFriend)
            {
                if(BasicPlayerInfoOps.Contains(friends, player))
                {
                    Debug.LogError("Server added friend but the data store thinks the player is already a friend");
                    return;
                }

                friends.Add(player);
            }
        }

        private static void on_send_friend_request(BasicPlayerInfo target, FriendRequestResult r)
        {
            if(r.IsNowPending)
            {
                if(BasicPlayerInfoOps.Contains(pending_friends_outgoing, target))
                {
                    Debug.LogError("Server added pending outgoing friend but the datastore thinks the player is already in the pending list.");
                    return;
                }
                pending_friends_outgoing.Add(target);
            }
        }
    }
}
