using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(EquipmentController))]
public class CharacterRotator : NetworkBehaviour {
    EquipmentController equipment;

    [SyncVar]
    Quaternion goal_y_rotation;
    [SyncVar(hook = "on_goal_x_changed")]
    float goal_x_rotation;

    SmoothFloat smooth_goal_x_rotation;

    public float RotationSpeed;
    public float MaxXRotation;
    public bool Demo;
    public float TargetYRotation;
    public float TargetXRotation;

    private void Start()
    {
        equipment = GetComponent<EquipmentController>();
    }

    private void OnValidate()
    {
        if (Demo && Application.isEditor)
        {
            set_rotation_impl(Quaternion.Euler(new Vector3
            {
                x = TargetXRotation,
                y = TargetYRotation
            }));
        }
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, goal_y_rotation, RotationSpeed * Time.deltaTime);
        var weapon = equipment.EquippedWeaponController;
        if(weapon != null)
        {
            weapon.XRotation = smooth_goal_x_rotation.Next(RotationSpeed);
        }
    }

    [Command]
    public void CmdSetRotation(Quaternion new_rot)
    {
        set_rotation_impl(new_rot);
    }

    private void set_rotation_impl(Quaternion new_rot)
    {
        var euler = new_rot.eulerAngles;
        goal_y_rotation = Quaternion.Euler(0, euler.y, 0);
        goal_x_rotation = euler.x;
        if (goal_x_rotation > 180)
            goal_x_rotation -= 360;
        else if (goal_x_rotation < -180)
            goal_x_rotation += 360;
        goal_x_rotation = Mathf.Clamp(goal_x_rotation, -MaxXRotation, MaxXRotation);
    }

    private void on_goal_x_changed(float new_)
    {
        smooth_goal_x_rotation.SetGoal(new_);
    }
}
