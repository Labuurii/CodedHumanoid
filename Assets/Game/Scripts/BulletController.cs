using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Animator))]
public class BulletController : NetworkBehaviour {
    Animator anim;
    bool exploding;

    public float Speed;
    public int Damage;
    public float DestroyDelay;

    public AudioSource OnHitAudio;

    // Use this for initialization
	void Awake() {
        anim = GetComponent<Animator>();
        if(isClient && !isServer)
        {
            GetComponent<Collider>().enabled = false;
        }

        BulletsStore.Instance.Bullets.Add(gameObject);
	}

    private void OnDestroy()
    {
        BulletsStore.Instance?.Bullets.Remove(gameObject);
    }

    private void FixedUpdate()
    {
        if (exploding)
            return;

        var pos = transform.position;
        pos += transform.forward * Speed * Time.fixedDeltaTime;
        transform.position = pos;
    }

    [Server]
    private void OnCollisionEnter(Collision collision)
    {
        if (exploding)
            return;

        exploding = true;
        Rpc_explode();

        var go = collision.gameObject;
        var dmgable = go.GetComponent<IDamagable>();
        if (dmgable != null)
        {
            dmgable.Damage(Damage);
        }
    }

    [ClientRpc]
    private void Rpc_explode()
    {
        exploding = true;
        anim.SetTrigger("explode");
        Destroy(gameObject, DestroyDelay);
        OnHitAudio?.Play();
    }
}
