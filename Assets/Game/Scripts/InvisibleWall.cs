using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleWall : MonoBehaviour {
	void Start () {
        var renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
            Destroy(renderer);
	}
}
