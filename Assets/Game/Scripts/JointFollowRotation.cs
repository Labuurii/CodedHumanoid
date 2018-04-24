using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Joint))]
public class JointFollowRotation : MonoBehaviour {
    Joint joint;

    public Vector3 Rotation = new Vector3(-110, -90, 180);
    public float MaxRotationSpeed;

	// Use this for initialization
	void Start () {
        joint = GetComponent<Joint>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        var body = joint.connectedBody;
        if(body != null)
        {
            var goal_rot = body.transform.rotation;
            goal_rot *= Quaternion.Euler(Rotation);

            var rot = Quaternion.RotateTowards(transform.rotation, goal_rot, MaxRotationSpeed * Time.deltaTime);

            transform.rotation = rot;
        }
	}
}
