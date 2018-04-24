using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using Grpc.Core;
using LoginServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LoginButton : MonoBehaviour {
    LoginClientLL client;
    Button btn;
    RepeatedField<LoginServices.Server> servers;

    public StatusText Status;
    public InputField Username, Password;

	// Use this for initialization
	void Start () {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(on_click);
	}

    private void OnDestroy()
    {
        if (client != null)
            client.Dispose();
    }

    private void on_click()
    {
        if (client != null)
            return;

        var name = Username.text;
        var pw = Password.text;
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(pw))
        {
            Status.Error("Username and password required."); //TRANSLATE
            return;
        }

        Debug.Log("PW: " + pw);

        btn.interactable = false;
        client = new LoginClientLL();
        client.OnDisconnected += on_disconnected;
        client.OnServerDisconnected += on_server_disconnected;
        client.OnLoginResult += on_login_result;
        client.DoLogin(name, pw);
    }

    private void on_login_result(bool success, RepeatedField<LoginServices.Server> servers)
    {
        if(success && servers != null && servers.Count > 0)
        {
            remove_client_listeners();
            this.servers = servers;
            SceneManager.sceneLoaded += on_lobby_scene_loaded;
            SceneManager.LoadScene("Lobby");
        } else
        {
            login_failed();

            Debug.Log("Failed to log in :(((");
        }
    }

    private void on_lobby_scene_loaded(Scene arg0, LoadSceneMode arg1)
    {
        SceneManager.sceneLoaded -= on_lobby_scene_loaded;
        BaseClientHL.Instance.EstablishConnection(client, servers);
    }

    private void login_failed()
    {
        btn.interactable = true;
        remove_client_listeners();
        client.Dispose();
        client = null;
    }

    private void remove_client_listeners()
    {
        client.OnDisconnected -= on_disconnected;
        client.OnServerDisconnected -= on_server_disconnected;
        client.OnLoginResult -= on_login_result;
    }

    private void on_server_disconnected()
    {
        login_failed();
        Status.Error("Internal error.");
        Debug.LogError("What instance server are we connected to?");
    }

    private void on_disconnected(bool internal_error, StatusCode? status)
    {
        login_failed();
        if(internal_error)
        {
            Status.Error("Can not connect to server. Internal error.");
        } else
        {
            Status.Error("Connection interrupted. " + (status.HasValue ? status.Value.ToString() : ""));
        }
    }
}
