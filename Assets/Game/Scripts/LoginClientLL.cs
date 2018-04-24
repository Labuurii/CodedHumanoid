using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Protobuf.Collections;
using Grpc.Core;
using UnityEngine;
using static LoginServices.LoginPublicService;

public class LoginClientLL : ClientBaseLL
{
    LoginPublicServiceClient client;

    public delegate void ServerDisconnectedCB();
    public ServerDisconnectedCB OnServerDisconnected;

    public delegate void LoginResultCB(bool success, RepeatedField<LoginServices.Server> servers);
    public LoginResultCB OnLoginResult;

    public LoginClientLL() : base(new Channel("localhost:32456", ChannelCredentials.Insecure))
    {
        client = new LoginPublicServiceClient(channel);
    }

    public void DoLogin(string username, string password)
    {
        var call = client.ConnectAndLogIn(new LoginServices.LogInAttempt
        {
            Username = username,
            Password = password
        });
        HandleEventsAsync(call, subscription_handler);
    }

    private Task subscription_handler(LoginServices.Event ev)
    {
        switch(ev.EventCase)
        {
            case LoginServices.Event.EventOneofCase.LoginResult:
                OnMainThread(() => OnLoginResult(ev.LoginResult.Success, ev.LoginResult.Servers));
                break;
            case LoginServices.Event.EventOneofCase.ServerDisconnected:
                OnMainThread(() => OnServerDisconnected());
                break;
            case LoginServices.Event.EventOneofCase.None:
                break;
            default:
                throw new System.Exception("Unhandled enum value " + ev.EventCase);
        }

        return null;
    }
}
