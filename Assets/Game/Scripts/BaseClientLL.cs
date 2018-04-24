using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArenaServices;
using Grpc.Core;
using UnityEngine;
using static ArenaServices.BaseService;

public class BaseClientLL : ClientBaseLL
{
    Guid session_token;
    BaseServiceClient client;
    Metadata metadata;

    public delegate void SuccessfulSubscriptionCB(Guid session_token);
    public SuccessfulSubscriptionCB OnSuccessfulSubscription;

    public BaseClientLL(Channel channel) : base(channel)
    {
        client = new BaseServiceClient(channel);
    }

    public void ConnectAsync()
    {
        HandleEventsAsync(client.Subscribe(new BaseServerSubscriptionAttempt()), handle_events);
    }

    private Task handle_events(EventBase ev)
    {
        switch (ev.EventCase)
        {
            case EventBase.EventOneofCase.SubscriptionSuccessful:
                var session_id_str = ev.SubscriptionSuccessful.SessionToken;
                if (!Guid.TryParse(session_id_str, out session_token))
                    throw new Exception("Could not parse the session token received from server!");
                MainThreadEventQueue.Enqueue(() => OnSuccessfulSubscription(session_token));
                if (metadata != null)
                    Debug.LogError("Subscriptionsuccessful is received multiple times!");
                var local_metadata = new Metadata(); //Local first because of thread concurrency
                GrpcHeaders.AddSessionToken(local_metadata, session_token);
                metadata = local_metadata;
                break;
            case EventBase.EventOneofCase.None:
                Debug.Log("Got none event.");
                break;
        }

        return null;
    }
}
