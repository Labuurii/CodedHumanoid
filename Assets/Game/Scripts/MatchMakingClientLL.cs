using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArenaServices;
using Grpc.Core;
using UnityEngine;
using static ArenaServices.MatchMakerService;

public class MatchMakingClientLL : ClientBaseLL
{
    MatchMakerServiceClient client;
    Metadata metadata;

    public delegate void NotAuthenticatedCB();
    public NotAuthenticatedCB OnNotAuthenticated;

    public delegate void StateChangedCB(QueueState new_state);
    public StateChangedCB OnStateChanged;

    public MatchMakingClientLL(Channel channel, Guid session_token) : base(channel)
    {
        client = new MatchMakerServiceClient(channel);
        metadata = new Metadata();
        GrpcHeaders.AddSessionToken(metadata, session_token);
    }

    public void ConnectAsync()
    {
        HandleEventsAsync(client.Subscribe(new SubscriptionAttempt(), metadata), handle_event);
    }

    public void Cancel()
    {
        SendRequestAsync(client.LeaveQueueAsync(new LeaveQueueAttempt
        {

        }, metadata), handle_result);
    }

    private void handle_result(QueueStateMsg res)
    {
        if(!res.Authenticated)
        {
            MainThreadEventQueue.Enqueue(() => OnNotAuthenticated());
        } else
        {
            MainThreadEventQueue.Enqueue(() => OnStateChanged(res.CurrentState));
        }
    }

    public void AnswerQueuePop(bool v)
    {
        SendRequestAsync(client.AnswerMatchAsync(new AnswerMatchAttempt
        {
            Accepted = v
        }, metadata), handle_result);
    }

    public void Queue(Arena arena, MatchMode mode, TeamSize team_size)
    {
        SendRequestAsync(client.QueueAsync(new QueueAttempt
        {
            Arena = arena,
            MatchMode = mode,
            TeamSize = team_size
        }, metadata), handle_result);
    }

    protected void SendRequestAsync<T>(AsyncUnaryCall<T> call, Action<T> result_handler)
    {
        Task.Run(async () =>
        {
            try
            {
                var res = await call;
                result_handler(res);
            } catch(RpcException e)
            {
                //TODO: Make a special notification?
                Debug.LogFormat("Got RpcException: '{0}'", e.Message);
            } catch(Exception e)
            {
                //This is a straight up error
                Debug.Log(e);
            }
        });
    }

    private Task handle_event(QueueStateMsg state)
    {
        if(!state.Authenticated)
        {
            MainThreadEventQueue.Enqueue(() => OnNotAuthenticated());
            return null;
        }

        switch(state.CurrentState)
        {
            case QueueState.ArenaJoinable: 
            case QueueState.AwaitingOtherPlayers: 
            case QueueState.InArena: 
            case QueueState.NotQueued: 
            case QueueState.Queued:
            case QueueState.QueuePopped:
                MainThreadEventQueue.Enqueue(() => OnStateChanged(state.CurrentState));
                break;
            default:
                throw new Exception("Unhandled enum value " + state.CurrentState);
        }

        return null;
    }
}
