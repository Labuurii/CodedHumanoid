using ArenaServices;
using Grpc.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AgarClientLL : ClientBaseLL
{
    readonly Arena3DService.Arena3DServiceClient client;

    public delegate void TransformChangedCB(Event3D_ObjectTransformChanged transform);
    public TransformChangedCB OnTransformChanged;

    public delegate void ObjectNotificationCB(Event3D_ObjectNotification notification);
    public ObjectNotificationCB OnObjectNotification;

    public AgarClientLL()
        : base(new Channel("localhost:8081", ChannelCredentials.Insecure))
    {
        client = new Arena3DService.Arena3DServiceClient(channel);
        HandleEventsAsync(client.Subscribe(new SubscriptionAttempt()), subscription_handler);
    }

    private Task subscription_handler(Event3D ev)
    {
        switch (ev.EventCase)
        {
            case Event3D.EventOneofCase.None:
                Debug.LogError("Got event without any event case");
                break;
            case Event3D.EventOneofCase.ObjectTransformChanged:
                OnMainThread(() => OnTransformChanged(ev.ObjectTransformChanged));
                break;
            case Event3D.EventOneofCase.ObjectNotification:
                OnMainThread(() => OnObjectNotification(ev.ObjectNotification));
                break;
            default:
                throw new Exception("Unhandled enum value " + ev.EventCase);
        }

        return null;
    }

    public void Connect()
    {
        //TODO: What is this even needed?
        channel.ConnectAsync().Wait();
        HandleEventsAsync(client.Subscribe(new SubscriptionAttempt()), subscription_handler);
    }
}
