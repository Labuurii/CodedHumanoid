using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponController : NetworkBehaviour {

    /// <summary>
    /// When this is false the weapon is in the bag.
    /// </summary>
    [SyncVar(hook = "on_net_enabled_changed")]
    bool net_enabled = true;
    GameObject wielder;
    Transform connector;

    public string Name;
    public Transform HandTrigger, HandSupport, PipeEnd;
    public WeaponType WeaponType;
    public int ClipSize;
    public float ClipRegenDelta;
    public float FireRateDelta;
    public AudioSource FireAudio;

    public GameObject BulletPrefab;


    public GameObject Wielder { get
        {
            return wielder;
        }
    }
    public Quaternion WielderRelativeRot{ get; set; }
    public float XRotation { get; set; }

    private void Start()
    {
        if(net_enabled)
        {
            DroppedWeaponsStore.Instance.WeaponsOnGround.Add(this);
        }
        on_net_enabled_changed(net_enabled);
    }

    private void OnDestroy()
    {
        DroppedWeaponsStore.Instance?.WeaponsOnGround.Remove(this);
    }

    private void LateUpdate()
    {
        if(wielder != null)
        {
            transform.position = connector.position;
            transform.rotation = wielder.transform.rotation * WielderRelativeRot * Quaternion.Euler(XRotation, 0, 0);
        } else
        {
            transform.rotation = Quaternion.identity;
        }
    }

    [Server]
    public void Use()
    {
        switch(WeaponType)
        {
            case WeaponType.Default:
                Debug.LogError("Tried to use default weapon.");
                break;
            case WeaponType.Rifle:
            case WeaponType.Pistol:
                if (PipeEnd != null)
                {
                    var go = Instantiate(BulletPrefab, PipeEnd.position, PipeEnd.rotation);
                    NetworkServer.Spawn(go);
                }
                break;
            default:
                throw new Exception("Unhandled enum value " + WeaponType);
        }
    }

    [Server]
    public bool IsNotPickedUp()
    {
        return wielder == null;
    }

    [Server]
    public void SetAsPickedUp(EquipmentController wielder)
    {
        this.wielder = wielder.gameObject;
        DroppedWeaponsStore.Instance.WeaponsOnGround.Remove(this);
    }

    public void SetConnector(Transform connector)
    {
        this.connector = connector;
    }

    public void UnWield()
    {
        wielder = null;
        connector = null;
        DroppedWeaponsStore.Instance.WeaponsOnGround.Add(this);
    }

    [Server]
    public void Destroy()
    {
        NetworkServer.Destroy(gameObject);
    }

    [Server]
    public void Show()
    {
        net_enabled = true;
    }

    [Server]
    public void Hide()
    {
        net_enabled = false;
    }

    private void on_net_enabled_changed(bool new_value)
    {
        gameObject.SetActive(new_value);
        if(!new_value) //Weapon is put in bag somehow
        {
            DroppedWeaponsStore.Instance.WeaponsOnGround.Remove(this);
        }
    }
}
