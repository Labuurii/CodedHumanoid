using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LocalPlayerCamera : NetworkBehaviour {
	void Start () {
		if(isLocalPlayer)
        {
            Camera.main.GetComponent<CameraFollow>().Target = transform;
        }
	}
}
