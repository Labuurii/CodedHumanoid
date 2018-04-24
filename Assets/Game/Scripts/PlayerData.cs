using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerData : NetworkBehaviour {
    public string DisplayName;

    private void Start()
    {
        if(!isLocalPlayer)
            OtherPlayersStore.Instance.Players.Add(this);
    }

    private void OnDestroy()
    {
        if(!isLocalPlayer)
            OtherPlayersStore.Instance.Players.Remove(this);
    }
}
