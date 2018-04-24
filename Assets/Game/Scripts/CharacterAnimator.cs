using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum WeaponType
{
    Default = 0,
    Rifle = 1,
    Pistol = 2
}

interface IWeaponAnim
{
    void OnIKAnim();
}

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class CharacterAnimator : MonoBehaviour {
    Animator anim;
    NavMeshAgent agent;
    SmoothVector3 left_hand_pos, right_hand_pos; 
    Transform left_hold_body, right_hold_body;

    public float HandSpeed, AimSpeed;


    public Transform LeftHoldBody
    {
        set
        {
            if(value != null)
            {
                left_hand_pos.SetValue(value.position);
                left_hold_body = value;
            } else
            {
                left_hold_body = null;
            }
        }
    }

    public Transform RightHoldBody
    {
        set
        {
            if(value != null)
            {
                right_hand_pos.SetValue(value.position);
                right_hold_body = value;
            } else
            {
                left_hold_body = null;
            }
        }
    }

    // Use this for initialization
    void Awake () {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
	}

    private void OnAnimatorIK(int layerIndex)
    {
        if(layerIndex == 0)
        {
            if(left_hold_body != null)
            {
                left_hand_pos.SetGoal(left_hold_body.position);
                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                anim.SetIKPosition(AvatarIKGoal.LeftHand, left_hand_pos.Next(HandSpeed));
                //anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                //anim.SetIKRotation(AvatarIKGoal.LeftHand, left_hold_body.rotation);
            }

            if(right_hold_body != null)
            {
                right_hand_pos.SetGoal(right_hold_body.position);
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                anim.SetIKPosition(AvatarIKGoal.RightHand, right_hand_pos.Next(HandSpeed));
                //anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                //anim.SetIKRotation(AvatarIKGoal.RightHand, right_hold_body.rotation);
            }
        }
    }
}
