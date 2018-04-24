using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterDamagable : MonoBehaviour, IDamagable {

    class TookDmgCB : UnityEvent<int> { }

    public int Multiplier = 1;

    public UnityEvent<int> OnTookDmg = new TookDmgCB();

    public void Damage(int dmg)
    {
        OnTookDmg.Invoke(Multiplier * dmg);
    }
}
