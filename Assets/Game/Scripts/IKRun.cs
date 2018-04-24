using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CyclicGrowAndShrink01
{
    float v;
    State s;

    enum State
    {
        Growing,
        Shrinking
    }

    public CyclicGrowAndShrink01(float start_value)
    {
        v = Mathf.Clamp01(start_value);
        s = State.Growing;
    }

    public float Next(float speed)
    {
        switch(s)
        {
            case State.Growing:
                v += speed * Time.deltaTime;
                v = Mathf.Clamp01(v);
                if (v == 1f)
                    s = State.Shrinking;
                break;
            case State.Shrinking:
                v -= speed * Time.deltaTime;
                v = Mathf.Clamp01(v);
                if (v == 0f)
                    s = State.Growing;
                break;
            default:
                throw new System.Exception("Unhandled enum value " + s);
        }

        return v;
    }
}

public struct SmoothFloat
{
    float v;
    float goal;

    public void SetGoal(float goal)
    {
        this.goal = goal;
    }

    public void SetValue(float v)
    {
        this.v = v;
    }

    public float Next(float speed)
    {
        v = Mathf.MoveTowards(v, goal, speed * Time.deltaTime);
        return v;
    }

    public float Goal()
    {
        return goal;
    }
}

public struct SmoothVector3
{
    SmoothFloat x, y, z;

    public void SetGoal(Vector3 g)
    {
        x.SetGoal(g.x);
        y.SetGoal(g.y);
        z.SetGoal(g.z);
    }

    public void SetValue(Vector3 v)
    {
        x.SetValue(v.x);
        y.SetValue(v.y);
        z.SetValue(v.z);
    }

    public Vector3 Next(float speed)
    {
        return new Vector3
        {
            x = x.Next(speed),
            y = y.Next(speed),
            z = z.Next(speed)
        };
    }

    public Vector3 Goal()
    {
        return new Vector3
        {
            x = x.Goal(),
            y = y.Goal(),
            z = z.Goal()
        };
    }
}

[RequireComponent(typeof(Animator))]
public class IKRun : MonoBehaviour {
    Animator anim;
    CyclicGrowAndShrink01 left_foot;
    CyclicGrowAndShrink01 right_foot = new CyclicGrowAndShrink01(1);

    public float Length;
    public float Height;
    public float Speed;
    public float Spacing;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}

    private void OnAnimatorIK(int layerIndex)
    {
        if(layerIndex == 0)
        {
            anim.SetIKPosition(AvatarIKGoal.LeftFoot, transform.position + transform.forward * Length + (-transform.up) * Height + (-transform.right) * Spacing);
            anim.SetIKPosition(AvatarIKGoal.RightFoot, transform.position + transform.forward * Length + (-transform.up) * Height + transform.right * Spacing);
            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, left_foot.Next(Speed));
            anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, right_foot.Next(Speed));
        }
    }
}
