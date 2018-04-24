using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using MainServerV2;
using ServerClientSharedV2;
using static MainServerV2.MainServiceV2;

namespace Assets.Game.Scripts
{
    public class MainServerClientV2LL : ClientBaseLL
    {
        const double DEADLINE_SEC = 5;

        MainServiceV2Client client;
        Metadata headers = new Metadata();

        public delegate void FriendAcceptedRequestCB(Event_FriendAcceptedRequest ev);
        public delegate void FriendOnlineStateChangedCB(Event_FriendOnlineStateChanged ev);
        public delegate void FriendRemovedYouCB(Event_FriendRemovedYou ev);
        public delegate void FriendRequestCB(Event_FriendRequest ev);
        public delegate void GotGroupInviteCB(Event_GotGroupInvite ev);
        public delegate void GroupDequeuedCB(Event_GroupDequeued ev);
        public delegate void GroupInviteTimedOut(Event_GroupInviteTimedOut ev);
        public delegate void GroupQueueStateChangedCB(Event_GroupQueueStateChanged ev);
        public delegate void LoginResultCB(Event_LoginResult ev);
        public delegate void PlayerAnsweredGroupInviteCB(Event_PlayerAnsweredGroupInvite ev);
        public delegate void PlayerLeftGroupCB(Event_PlayerLeftGroup ev);
        public delegate void QueuePoppedCB(Event_QueuePopped ev);
        public delegate void ReceivedMessageCB(Event_ReceivedMessage ev);

        public delegate void VersionCB(VersionResult r);
        public delegate void SendFriendRequestCB(BasicPlayerInfo player, FriendRequestResult r);
        public delegate void AnswerFriendRequestCB(BasicPlayerInfo player, AnswerFriendRequestResult r);
        public delegate void RemoveFriendCB(BasicPlayerInfo player, RemoveFriendResult r);
        public delegate void IgnorePlayerCB(BasicPlayerInfo player, IgnorePlayerResult r);
        public delegate void UnIgnorePlayerCB(BasicPlayerInfo player, UnIgnorePlayerResult r);
        public delegate void InviteToGroupCB(BasicPlayerInfo player, InviteToGroupResult r);
        public delegate void AnswerGroupInviteCB(AnswerGroupInviteResult r);
        public delegate void LeaveGroupCB(LeaveGroupResult r);
        public delegate void SendMessageCB(SendMessageResult r);
        public delegate void QueueCB(QueueResult r);
        public delegate void LeaveQueueCB(LeaveQueueResult r);

        public FriendAcceptedRequestCB OnFriendAcceptedRequest;
        public FriendOnlineStateChangedCB OnFriendOnlineStateChanged;
        public FriendRemovedYouCB OnFriendRemovedYou;
        public FriendRequestCB OnFriendRequest;
        public GotGroupInviteCB OnGroupInvite;
        public GroupDequeuedCB OnGroupDequeued;
        public GroupInviteTimedOut OnGroupInviteTimedOut;
        public GroupQueueStateChangedCB OnGroupQueueStateChanged;
        public LoginResultCB OnLoginResult;
        public PlayerAnsweredGroupInviteCB OnPlayerAnsweredGroupInvite;
        public PlayerLeftGroupCB OnPlayerLeftGroup;
        public QueuePoppedCB OnQueuePopped;
        public ReceivedMessageCB OnReceivedMessage;

        public VersionCB OnVersion;
        public SendFriendRequestCB OnSendFriendRequest;
        public AnswerFriendRequestCB OnAnswerFriendRequest;
        public RemoveFriendCB OnRemoveFriend;
        public IgnorePlayerCB OnIgnorePlayer;
        public UnIgnorePlayerCB OnUnIgnorePlayer;
        public InviteToGroupCB OnInviteToGroup;
        public AnswerGroupInviteCB OnAnswerGroupInvite;
        public LeaveGroupCB OnLeaveGroup;
        public SendMessageCB OnSendMessage;
        public QueueCB OnQueue;
        public LeaveQueueCB OnLeaveQueue;

        public MainServerClientV2LL(Channel channel) : base(channel)
        {
            client = new MainServiceV2Client(channel);
        }

        public void LoginAndStartHandleEvents(string username, string password)
        {
            HandleEventsAsync(client.Subscribe(new SubscriptionAttempt
            {
                Username = username,
                Password = password
            }), handle_events);

            OnLoginResult += on_login_result;
        }

        private void on_login_result(Event_LoginResult ev)
        {
            if(ev.Success)
            {
                if(headers.Count > 0)
                {
                    UnityEngine.Debug.LogError("on_login_result seems to be called multiple times...");
                }
                headers.Add(new Metadata.Entry(Constants.SessionTokenHeader, ev.Token.ToArray()));
            }
        }

        public void Version()
        {
            make_rpc(client.VersionAsync(new VersionAttempt(), deadline: make_deadline(), headers: headers), (d) => OnVersion(d));
        }

        public void SendFriendRequest(BasicPlayerInfo player)
        {
            make_rpc(client.SendFriendRequestAsync(new FriendRequestAttempt
            {
                PlayerId = player.PermanentId
            }, deadline: make_deadline(), headers: headers), (d) => OnSendFriendRequest(player, d));
        }

        public void AnswerFriendRequest(BasicPlayerInfo player)
        {
            make_rpc(client.AnswerFriendRequestAsync(new AnswerFriendRequestAttempt
            {
                From = player.PermanentId
            }, deadline: make_deadline(), headers: headers), (d) => OnAnswerFriendRequest(player, d));
        }

        public void RemoveFriend(BasicPlayerInfo player)
        {
            make_rpc(client.RemoveFriendAsync(new RemoveFriendAttempt
            {
                PlayerId = player.PermanentId
            }, deadline: make_deadline(), headers: headers), (d) => OnRemoveFriend(player, d));
        }

        public void IgnorePlayer(BasicPlayerInfo player)
        {
            make_rpc(client.IgnorePlayerAsync(new IgnorePlayerAttempt
            {
                PlayerId = player.PermanentId
            }, deadline: make_deadline(), headers: headers), (d) => OnIgnorePlayer(player, d));
        }

        public void UnIgnorePlayer(BasicPlayerInfo player)
        {
            make_rpc(client.UnIgnorePlayerAsync(new UnIgnorePlayerAttempt
            {
                PlayerId = player.PermanentId
            }, deadline: make_deadline(), headers: headers), (d) => OnUnIgnorePlayer(player, d));
        }

        public void InviteToGroup(BasicPlayerInfo player)
        {
            make_rpc(client.InviteToGroupAsync(new InviteToGroupAttempt
            {
                PlayerId = player.PermanentId
            }, deadline: make_deadline(), headers: headers), (d) => OnInviteToGroup(player, d));
        }

        public void AnswerGroupInvite(bool accepted)
        {
            make_rpc(client.AnswerGroupInviteAsync(new AnswerGroupInviteAttempt
            {
                Accepted = accepted
            }, deadline: make_deadline(), headers: headers), (d) => OnAnswerGroupInvite(d));
        }

        public void LeaveGroup()
        {
            make_rpc(client.LeaveGroupAsync(new LeaveGroupAttempt
            {
                
            }, deadline: make_deadline(), headers: headers), (d) => OnLeaveGroup(d));
        }

        public void SendGroupMessage(string message)
        {
            make_rpc(client.SendMessageAsync(new SendMessageAttempt
            {
                Msg = message,
                GroupTarget = new GroupTarget {}
            }, deadline: make_deadline(), headers: headers), (d) => OnSendMessage(d));
        }

        public void SendFriendMessage(string message, long permanent_player_id)
        {
            make_rpc(client.SendMessageAsync(new SendMessageAttempt
            {
                Msg = message,
                FriendTarget = new FriendTarget
                {
                    PlayerId = permanent_player_id
                }
            }, deadline: make_deadline(), headers: headers), (d) => OnSendMessage(d));
        }

        public void Queue(MatchMode match_mode)
        {
            make_rpc(client.QueueAsync(new QueueAttempt
            {
                MatchMode = match_mode
            }, deadline: make_deadline(), headers: headers), (d) => OnQueue(d));
        }

        public void LeaveQueue()
        {
            make_rpc(client.LeaveQueueAsync(new LeaveQueueAttempt
            {
                
            }, deadline: make_deadline(), headers: headers), (d) => OnLeaveQueue(d));
        }

        private DateTime make_deadline()
        {
            return DateTime.Now + TimeSpan.FromSeconds(DEADLINE_SEC);
        }

        private void make_rpc<T>(AsyncUnaryCall<T> response, Action<T> handler)
        {
            Task.Run(async () =>
            {
                var result = await response;
                MainThreadEventQueue.Enqueue(() => handler(result));
            });
        }

        private Task handle_events(Event ev)
        {
            switch(ev.EventCase)
            {
                case Event.EventOneofCase.None:
                    UnityEngine.Debug.Log("Got a none event from server.");
                    break;
                case Event.EventOneofCase.FriendAcceptedRequest:
                    MainThreadEventQueue.Enqueue(() => OnFriendAcceptedRequest(ev.FriendAcceptedRequest)); break;
                case Event.EventOneofCase.FriendOnlineStateChanged:
                    MainThreadEventQueue.Enqueue(() => OnFriendOnlineStateChanged(ev.FriendOnlineStateChanged)); break;
                case Event.EventOneofCase.FriendRemovedYou:
                    MainThreadEventQueue.Enqueue(() => OnFriendRemovedYou(ev.FriendRemovedYou)); break;
                case Event.EventOneofCase.FriendRequest:
                    MainThreadEventQueue.Enqueue(() => OnFriendRequest(ev.FriendRequest)); break;
                case Event.EventOneofCase.GotGroupInvite:
                    MainThreadEventQueue.Enqueue(() => OnGroupInvite(ev.GotGroupInvite)); break;
                case Event.EventOneofCase.GroupDequeued:
                    MainThreadEventQueue.Enqueue(() => OnGroupDequeued(ev.GroupDequeued)); break;
                case Event.EventOneofCase.GroupInviteTimedOut:
                    MainThreadEventQueue.Enqueue(() => OnGroupInviteTimedOut(ev.GroupInviteTimedOut)); break;
                case Event.EventOneofCase.GroupQueueStateChanged:
                    MainThreadEventQueue.Enqueue(() => OnGroupQueueStateChanged(ev.GroupQueueStateChanged)); break;
                case Event.EventOneofCase.LoginResult:
                    MainThreadEventQueue.Enqueue(() => OnLoginResult(ev.LoginResult)); break;
                case Event.EventOneofCase.PlayerAnsweredGroupInvite:
                    MainThreadEventQueue.Enqueue(() => OnPlayerAnsweredGroupInvite(ev.PlayerAnsweredGroupInvite)); break;
                case Event.EventOneofCase.PlayerLeftGroup:
                    MainThreadEventQueue.Enqueue(() => OnPlayerLeftGroup(ev.PlayerLeftGroup)); break;
                case Event.EventOneofCase.QueuePopped:
                    MainThreadEventQueue.Enqueue(() => OnQueuePopped(ev.QueuePopped)); break;
                case Event.EventOneofCase.ReceivedMessage:
                    MainThreadEventQueue.Enqueue(() => OnReceivedMessage(ev.ReceivedMessage)); break;
                default:
                    throw new Exception("Unhandled enum value " + ev.EventCase);
            }

            return null;
        }
    }
}
