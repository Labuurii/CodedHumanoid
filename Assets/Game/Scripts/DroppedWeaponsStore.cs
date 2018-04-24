using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedWeaponsStore : MonoBehaviour {
    public List<WeaponController> WeaponsOnGround = new List<WeaponController>();

    public static DroppedWeaponsStore Instance;

    private void Awake()
    {
        Debug.Assert(Instance == null, "There are multiple instances of DroppedWeaponsStore.");
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
