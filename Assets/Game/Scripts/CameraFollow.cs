using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    SmoothVector3 smooth_pos;


    public Transform Target;
    public Vector3 Offset;
    public float Speed;
    public float SpeedDistanceMul;
    public float SnapDistance;
    public float LookAhead;

    private void LateUpdate()
    {
        if(Target != null)
        {
            var goal = calc_goal();
            var d = Vector3.Distance(transform.position, goal);
            if (d > SnapDistance)
            {
                transform.position = goal;
            } else
            {
                smooth_pos.SetGoal(goal);
                smooth_pos.SetValue(transform.position);
                transform.position = smooth_pos.Next(Speed + d * SpeedDistanceMul);
            }

            var look_pos = Target.transform.position + Target.transform.forward * LookAhead;
            transform.LookAt(look_pos);
        }
    }

    private Vector3 calc_goal()
    {
        var goal = Target.transform.position;
        goal += Target.transform.forward * Offset.z;
        goal += Target.transform.right * Offset.x;
        goal += Target.transform.up * Offset.y;
        return goal;
    }
}
