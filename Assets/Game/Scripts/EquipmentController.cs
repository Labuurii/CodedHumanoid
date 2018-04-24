using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

[RequireComponent(typeof(IKEquip))]
[RequireComponent(typeof(Bag))]
public class EquipmentController : NetworkBehaviour {
    IKEquip ik_equip;
    Bag bag;

    WeaponController equipped_weapon;
    [SyncVar(hook = "client_clip_size_changed")]
    int clip_size;
    int max_clip_size;
    float fire_rate_delta;
    float time_since_last_fired_shot;
    CyclicTimer clip_regen_timer;

    public struct EquipmentData
    {
        public GameObject equipped_weapon;
    }

    public bool LeftHanded;
    public Joint LeftShoulderFront, RightShoulderFront;

    class ClipSizeChangedCB : UnityEvent<int> { }

    public UnityEvent<int> OnClipSizeChanged = new ClipSizeChangedCB();

    public GameObject EquippedWeapon { get
        {
            return equipped_weapon?.gameObject;
        }
    }

    public WeaponController EquippedWeaponController {
        get
        {
            return equipped_weapon;
        }
    }

    private void Start()
    {
        ik_equip = GetComponent<IKEquip>();
        bag = GetComponent<Bag>();

        if(isClient && !isLocalPlayer)
        {
            if (NetworkClient.allClients.Count != 1)
                return;
            NetworkClient.allClients[0].Send((short)NetMessages.RequestIds.PlayerData, new NetMessages.PlayerDataAttempt
            {
                net_id = GetComponent<NetworkIdentity>()
            });
        }

        bag.OnRemove.AddListener(on_remove_weapon);
        bag.OnAdd.AddListener(on_add_weapon);
    }

    private void on_add_weapon(WeaponController weapon, GameObject go)
    {
        weapon.SetAsPickedUp(this);
    }

    private void on_remove_weapon(GameObject go)
    {
        if(equipped_weapon != null && equipped_weapon.gameObject == go)
        {
            equipped_weapon.UnWield();
            equipped_weapon = null;
            ik_equip.DropWeapon();
        }
    }

    [TargetRpc]
    public void TargetSendPlayerData(NetworkConnection target, EquipmentData data)
    {
        if(data.equipped_weapon != null)
        {
            var weapon = data.equipped_weapon.GetComponent<WeaponController>();
            if(weapon == null)
            {
                Debug.LogError("equipped_weapon is not null but it is not a weapon controller.");
            } else
            {
                ik_equip.SwapWeapon(weapon);
            }
        }
    }

    private void LateUpdate()
    {
        if (!isServer)
            return;

        if(equipped_weapon != null)
        {
            if(clip_regen_timer.HasPassedLastFrame())
            {
                ++clip_size;
                if (clip_size > max_clip_size)
                {
                    clip_size = max_clip_size;
                } else
                {
                    OnClipSizeChanged.Invoke(clip_size);
                }
            }

            time_since_last_fired_shot += Time.deltaTime;
        }
    }

    [Client]
    public void EquipWeapon(WeaponController weapon)
    {
        if (!can_be_equipped(weapon))
            return;
        Cmd_equip_weapon_impl(weapon.gameObject);
    }

    [Command]
    public void Cmd_equip_weapon_impl(GameObject go)
    {
        var weapon = go.GetComponent<WeaponController>();
        if (!can_be_equipped(weapon))
            return;

        equipped_weapon = weapon;
        clip_size = 0;
        OnClipSizeChanged.Invoke(clip_size);
        max_clip_size = weapon.ClipSize;
        fire_rate_delta = time_since_last_fired_shot = weapon.FireRateDelta;
        clip_regen_timer = new CyclicTimer(weapon.ClipRegenDelta);

        ik_equip.SwapWeapon(weapon);
        Rpc_change_weapon(weapon.gameObject);
    }

    private bool can_be_equipped(WeaponController weapon)
    {
        return weapon != null && weapon.Wielder == gameObject && bag.ContainsWeapon(weapon);
    }

    [Command]
    public void CmdUseWeapon()
    {
        if (equipped_weapon != null)
        {
            if(clip_size > 0)
            {
                if(time_since_last_fired_shot >= fire_rate_delta)
                {
                    time_since_last_fired_shot = 0;
                    --clip_size;
                    OnClipSizeChanged.Invoke(clip_size);
                    equipped_weapon.Use();
                    equipped_weapon.FireAudio?.Play();
                }
            }
        }
    }

    void client_clip_size_changed(int new_clip_size)
    {
        OnClipSizeChanged.Invoke(new_clip_size);
    }

    [ClientRpc]
    void Rpc_change_weapon(GameObject obj)
    {
        if(!isServer)
        {
            var weapon = obj.GetComponent<WeaponController>();
            if (weapon != null)
            {
                ik_equip.SwapWeapon(weapon);
            }
            else
            {
                Debug.LogError("Rpc_change_weapon called without a WeaponController.");
            }
        }
    }
}
