using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterAnimator))]
public class IKEquip : NetworkBehaviour {
    Animator anim;
    CharacterAnimator char_anim;
    StateE state;
    float equip_time;
    SmoothVector3 current_weapon_rotation;
    WeaponController old_weapon;
    WeaponController current_weapon;

    enum StateE
    {
        InActive,
        Reaching,
        Equipping
    }

    public bool LeftHanded;
    public Transform BagPosition, LeftElbowPos, RightElbowPos, LeftShoulderFront, RightShoulderFront, PistolPosition;
    public float Speed, PullUpSpeed;
    public Vector3 RifleStartRotation;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        char_anim = GetComponent<CharacterAnimator>();
        current_weapon_rotation.SetGoal(Vector3.zero);
	}

    private void OnAnimatorIK(int layerIndex)
    {
        if(layerIndex == 0)
        {
            switch(state)
            {
                case StateE.Reaching:
                    {
                        var hand_goal = LeftHanded ? AvatarIKGoal.LeftHand : AvatarIKGoal.RightHand;
                        var elbow_goal = LeftHanded ? AvatarIKHint.LeftElbow : AvatarIKHint.RightElbow;
                        var elbow_transform = LeftHanded ? LeftElbowPos : RightElbowPos;

                        equip_time = Mathf.MoveTowards(equip_time, 1, Speed * Time.deltaTime);
                        anim.SetIKPosition(hand_goal, BagPosition.position);
                        anim.SetIKPositionWeight(hand_goal, equip_time);
                        anim.SetIKHintPosition(elbow_goal, elbow_transform.position);
                        anim.SetIKHintPositionWeight(elbow_goal, equip_time);
                        if (equip_time == 1f)
                        {
                            state = StateE.Equipping;
                            old_weapon?.Destroy();
                            current_weapon.Show();

                            Vector3 start_rot;
                            switch (current_weapon.WeaponType)
                            {
                                case WeaponType.Rifle:
                                case WeaponType.Pistol:
                                    start_rot = RifleStartRotation;
                                    if(LeftHanded)
                                    {
                                        start_rot.x = -start_rot.x;
                                    }
                                    break;
                                case WeaponType.Default:
                                    throw new System.Exception("A default weapon snuk into the IKEquip data. Invariant is broken.");
                                default:
                                    throw new System.Exception("Unhandled enum value " + current_weapon.WeaponType);
                            }

                            current_weapon.WielderRelativeRot = Quaternion.Euler(start_rot);
                            current_weapon_rotation.SetValue(start_rot);

                            if (LeftHanded)
                            {
                                char_anim.RightHoldBody = current_weapon.HandSupport;
                                char_anim.LeftHoldBody = current_weapon.HandTrigger;
                            }
                            else
                            {
                                char_anim.RightHoldBody = current_weapon.HandTrigger;
                                char_anim.LeftHoldBody = current_weapon.HandSupport;
                            }
                        }
                    }
                    break;
                case StateE.Equipping:
                    {
                        var next = current_weapon_rotation.Next(PullUpSpeed);
                        current_weapon.WielderRelativeRot = Quaternion.Euler(next);
                        if(next == current_weapon_rotation.Goal())
                        {
                            state = StateE.InActive;
                        }
                    }
                    break;
                case StateE.InActive:
                    break; //Nothing to do
                default:
                    throw new System.Exception("Unhandled enum value " + state);
            }
        }
    }

    public WeaponController SwapWeapon(WeaponController weapon)
    {
        if (weapon.WeaponType == WeaponType.Default)
            throw new System.ArgumentException("weapon can not be Default");

        switch(weapon.WeaponType)
        {
            case WeaponType.Default:
                throw new System.Exception("Unexpected Default weapon type.");
            case WeaponType.Rifle:
                var connector = LeftHanded ? LeftShoulderFront : RightShoulderFront;
                weapon.SetConnector(connector);
                break;
            case WeaponType.Pistol:
                weapon.SetConnector(PistolPosition);
                break;
            default:
                throw new System.Exception("Unhandled enum value " + weapon.WeaponType);
        }

        
        var old_weapon = this.old_weapon = current_weapon;
        current_weapon = weapon;
        old_weapon?.Hide();
        weapon.Hide();
        equip_time = 0;
        state = StateE.Reaching;

        return old_weapon;
    }

    public void DropWeapon()
    {
        state = StateE.InActive;
    }
}
