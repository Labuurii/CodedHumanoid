using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class FreeForAllNetManager : NetworkManager
{
    private void Start()
    {
        NetworkServer.RegisterHandler((short)NetMessages.RequestIds.PlayerData, on_player_data_request);
    }

    private void on_player_data_request(NetworkMessage netMsg)
    {
        var attempt = netMsg.ReadMessage<NetMessages.PlayerDataAttempt>();
        if(attempt == null)
        {
            Debug.LogError("Client tried to make a player data request without any object id.");
            return;
        }

        if (netMsg.conn.playerControllers.Count != 1)
            return;

        var target_eq = attempt?.net_id?.GetComponent<EquipmentController>();
        if (target_eq == null)
            return;

        target_eq.TargetSendPlayerData(netMsg.conn, new EquipmentController.EquipmentData
        {
            equipped_weapon = target_eq.EquippedWeapon
        });
    }
}
