using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsStore : MonoBehaviour {
    public List<GameObject> Bullets = new List<GameObject>();

    public static BulletsStore Instance;

	void Awake() {
        Debug.Assert(Instance == null, "There are multiple Bullet instances.");
        Instance = this;
	}

    private void OnDestroy()
    {
        Instance = null;
    }
}
