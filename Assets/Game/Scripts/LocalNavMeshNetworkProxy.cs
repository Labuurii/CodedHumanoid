using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

[RequireComponent(typeof(NavMeshAgent))]
public class LocalNavMeshNetworkProxy : NetworkBehaviour {
    NavMeshAgent agent;
    Vector3 last_destination;

    public float AngleSendThreshold;

    struct TransformData
    {
        internal Vector3 destination, pos;
    }

	void Awake () {
        agent = GetComponent<NavMeshAgent>();
        if (!isServer)
        {
            Cmd_ask_about_destination();
        }
	}

    [Client]
    public void SetDestination(Vector3 pos)
    {
        if(!isLocalPlayer)
        {
            Debug.Log("Expected to be called on local player only!");
            return;
        }

        Cmd_set_destination(pos);
    }

    [Command]
    private void Cmd_set_destination(Vector3 destination)
    {
        agent.destination = destination;
        Rpc_transform_changed(create_transform_data());
    }

    [ClientRpc]
    private void Rpc_transform_changed(TransformData t)
    {
        client_set_transform(t);
    }

    [Command]
    private void Cmd_ask_about_destination()
    {
        Target_send_destination(connectionToClient, create_transform_data());
    }

    [TargetRpc]
    private void Target_send_destination(NetworkConnection target, TransformData t)
    {
        client_set_transform(t);
    }

    [Client]
    private void client_set_transform(TransformData t)
    {
        agent.destination = t.destination;
        transform.position = t.pos;
    }

    [Server]
    private TransformData create_transform_data()
    {
        return new TransformData
        {
            destination = agent.destination,
            pos = transform.position
        };
    }
}
