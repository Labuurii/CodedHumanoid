using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class CharacterHP : NetworkBehaviour {
    int hp = 100;

#if UNITY_EDITOR
    public List<GameObject> FoundDamagable = new List<GameObject>();
#endif

    class HPChangedCB : UnityEvent<int> { }

    public UnityEvent<int> OnHPChanged = new HPChangedCB();
    public UnityEvent OnDead = new UnityEvent();

    private void Start()
    {
        if (!isServer)
            return;

        var result = GetComponentsInChildren<CharacterDamagable>();
#if UNITY_EDITOR
        FoundDamagable.Capacity = result.Length;
        foreach (var r in result)
            FoundDamagable.Add(r.gameObject);
#endif
        foreach (var dmgable in result)
        {
            dmgable.OnTookDmg.AddListener(on_took_dmg);
        }
    }

    [Server]
    private void on_took_dmg(int dmg)
    {
        var old_hp = hp;
        hp -= dmg;
        if(hp <= 0)
        {
            hp = 0;
            OnDead.Invoke();
        }

        if(old_hp != hp) //Most stupid implementation of all time. Can be alot more minimalistic. But meeh don't care here.
        {
            OnHPChanged.Invoke(hp);
        }
    }
}
