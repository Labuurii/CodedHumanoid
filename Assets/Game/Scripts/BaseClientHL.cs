using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.Collections;
using Grpc.Core;
using LoginServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseClientHL : MonoBehaviour
{
    static BaseClientHL _instance;
    LoginClientLL login_client;
    private LoginServices.Server selected_server;
    BaseClientLL base_service;

    public MatchMakingClientHL MatchMaker;

    public static BaseClientHL Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            base_service?.Dispose();
            login_client?.Dispose();
            _instance = null;
        }
    }

    public void EstablishConnection(LoginClientLL login_service, RepeatedField<LoginServices.Server> servers)
    {
        Debug.Assert(servers.Count > 0);
        Debug.Assert(base_service == null, "A base service operation is already taking place...");
        Debug.Assert(login_service == null, "Internal error. login service and base service always has to be in sync...");

        login_service.OnDisconnected += on_login_client_disconnected;
        login_service.OnServerDisconnected += on_main_server_caused_disconnection;

        var selected_server_idx = MainThread<System.Random>.Get().Next(0, servers.Count - 1);
        selected_server = servers[selected_server_idx];
        base_service = new BaseClientLL(new Channel(selected_server.Url, ChannelCredentials.Insecure)); //TODO: SSL, Compression
        base_service.OnDisconnected += on_base_server_disconnected;
        base_service.OnSuccessfulSubscription += on_successful_subscription;
        base_service.ConnectAsync();
    }

    private void on_successful_subscription(Guid session_token)
    {
        MatchMaker.Connect(new Channel(selected_server.Url, ChannelCredentials.Insecure), session_token); 
        //This should be safe to do without ssl. Even though speed is not necessary here.
    }

    private void on_base_server_disconnected(bool internal_error, StatusCode? status)
    {
        shut_down_services();
        throw new NotImplementedException(); //TODO:
    }

    private void shut_down_services()
    {
        if (base_service != null)
        {
            base_service.Dispose();
            base_service = null;
        }
        if(login_client != null)
        {
            login_client.Dispose();
            login_client = null;
        }
        SceneManager.LoadScene("Login");
    }

    private void on_main_server_caused_disconnection()
    {
        shut_down_services();
        throw new NotImplementedException(); //TODO:
    }

    private void on_login_client_disconnected(bool internal_error, StatusCode? status)
    {
        shut_down_services();
        throw new NotImplementedException(); //TODO:
    }
}
