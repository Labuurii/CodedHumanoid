using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

[RequireComponent(typeof(EquipmentController))]
public class Bag : NetworkBehaviour {

    List<WeaponController> bag = new List<WeaponController>();

    public int MaxBagSize;
    public float PickUpRange;

    class OnAddCB : UnityEvent<WeaponController, GameObject> { }
    class OnRemoveCB : UnityEvent<GameObject> { }

    public UnityEvent<WeaponController, GameObject> OnAdd = new OnAddCB();
    public UnityEvent<GameObject> OnRemove = new OnRemoveCB();
    public UnityEvent OnPickUpFailed = new UnityEvent();
    public UnityEvent OnDropFailed = new UnityEvent();

    [Client]
    public void PickUp(WeaponController weapon)
    {
        if (weapon == null)
            return;
        if (!can_be_picked_up(weapon))
            return;
        Cmd_pick_up(weapon.gameObject);
    }

    [Command]
	private void Cmd_pick_up(GameObject go)
    {
        var weapon = go.GetComponent<WeaponController>();
        if (weapon == null)
        {
            Debug.LogError("Can only pick up weapons currently.");
            Target_fire_pick_up_failed(connectionToClient);
            return;
        }

        if (!weapon.IsNotPickedUp() || !can_be_picked_up(weapon))
        {
            Target_fire_pick_up_failed(connectionToClient);
            return;
        }

        SpawnMaster.Instance.WeaponWasPickedUp();
        if (isLocalPlayer || isServer)
        {
            Target_fire_on_add(connectionToClient, go);
        } else
        {
            bag.Add(weapon);
            OnAdd.Invoke(weapon, go);
            Target_fire_on_add(connectionToClient, go);
        }
        weapon.Hide(); //Is the object sent anyways on the line above?
    }

    private bool can_be_picked_up(WeaponController weapon)
    {
        return bag.Count < MaxBagSize && Vector3.Distance(transform.position, weapon.transform.position) < PickUpRange;
    }

    [Client]
    public void Drop(WeaponController weapon)
    {
        if (!can_drop_weapon(weapon))
            return;
        Cmd_drop(weapon.gameObject);
    }

    [Command]
    private void Cmd_drop(GameObject go)
    {
        if (go == null)
        {
            Debug.LogError("Cmd_drop called without any game object.");
            return;
        }
        var weapon = go.GetComponent<WeaponController>();
        if (!can_drop_weapon(weapon))
        {
            Debug.LogError("GameObject is not a picked up weapon.");
            Target_fire_drop_failed(connectionToClient);
            return;
        }

        var rot = Quaternion.Euler(new Vector3
        {
            y = UnityEngine.Random.Range(0, 180)
        });
        var pos = transform.position; //TODO: Make sure weapon is placed on ground?
        weapon.Show();

        if (isLocalPlayer && isServer)
        {
            Target_fire_on_remove(connectionToClient, weapon.gameObject, pos, rot);
        }
        else
        {
            bag.Remove(weapon);
            SpawnMaster.Instance.WeaponWasDropped();
            Target_fire_on_remove(connectionToClient, weapon.gameObject, pos, rot);
        }
    }

    private bool can_drop_weapon(WeaponController weapon)
    {
        return weapon != null && bag.Contains(weapon);
    }

    [Server]
    public WeaponController GetWeapon(int idx)
    {
        if(idx >= bag.Count)
        {
            Debug.LogError("Tried to get weapon out of range.");
            return null;
        }

        var weapon = bag[idx];
        return weapon.GetComponent<WeaponController>();
    }

    [TargetRpc]
    void Target_fire_on_remove(NetworkConnection target, GameObject go, Vector3 pos, Quaternion rot)
    {
        var weapon = go.GetComponent<WeaponController>();
        if(weapon == null)
        {
            Debug.Log("Target_fire_on_remove called without any valid weapon controller");
            return;
        }
        bag.Remove(weapon);
        SpawnMaster.Instance.WeaponWasDropped();
        client_remove_sideeffects(go, pos, rot);
    }

    public bool ContainsWeapon(WeaponController weapon)
    {
        return bag.Contains(weapon);
    }

    private void client_remove_sideeffects(GameObject go, Vector3 pos, Quaternion rot)
    {
        go.transform.position = pos;
        go.transform.rotation = rot;
        OnRemove.Invoke(go);
    }

    [TargetRpc]
    private void Target_fire_drop_failed(NetworkConnection target)
    {
        OnDropFailed.Invoke();
    }

    [TargetRpc]
    private void Target_fire_pick_up_failed(NetworkConnection target)
    {
        OnPickUpFailed.Invoke();
    }

    [TargetRpc]
    private void Target_fire_on_add(NetworkConnection target, GameObject go)
    {
        var weapon = go.GetComponent<WeaponController>();
        if(weapon == null)
        {
            Debug.LogError("Target_fire_on_add called with a gameobject which is not a weapon.");
            return;
        }
        bag.Add(weapon);
        OnAdd.Invoke(weapon, go);
    }

    public List<WeaponController> GetWeapons()
    {
        return bag;
    }
}