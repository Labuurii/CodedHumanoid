using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnMaster : NetworkBehaviour {
    int weapons_on_ground;

    public int MaxWeapons;
    public GameObject[] WeaponPrefabs;

    public static SpawnMaster Instance { get; private set; }

    private void Start()
    {
        Debug.Assert(Instance == null, "There are multiple SpawnMasters running at the same time.");
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    [Server]
    public void WeaponWasPickedUp()
    {
        if (--weapons_on_ground < 0)
            Debug.LogError("WeaponsWasPickedUp is called more often than weapons are picked up. Or weapons are picked up multiple times.");
    }

    [Server]
    public void WeaponWasDropped()
    {
        ++weapons_on_ground;
    }

    [Server]
    public bool ShouldSpawnNewWeapon()
    {
        return weapons_on_ground < MaxWeapons;
    }

    [Server]
    public void SpawnRandomWeapon(Vector3 pos)
    {
        if (WeaponPrefabs == null || WeaponPrefabs.Length == 0)
            return;

        var rot = new Vector3
        {
            y = Random.Range(0, 180)
        };

        var idx = Random.Range(0, WeaponPrefabs.Length);
        var go = Instantiate(WeaponPrefabs[idx], pos, Quaternion.Euler(rot), null);
        NetworkServer.Spawn(go);
        ++weapons_on_ground;
    }
}
