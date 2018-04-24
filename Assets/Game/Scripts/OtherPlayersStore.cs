using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayersStore : MonoBehaviour {
    public List<PlayerData> Players = new List<PlayerData>();

    public static OtherPlayersStore Instance;

	void Awake() {
        Debug.Assert(Instance == null, "There are multiple instances of the OtherPlayersStore.");
        Instance = this;
	}

    private void OnDestroy()
    {
        Instance = null;
    }
}
