using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class Conv
{
    public static Vector3 ToUnityType(ArenaServices.Vector3 v)
    {
        return new Vector3
        {
            x = v.X,
            y = v.Y,
            z = v.Z,
        };
    }

    public static Vector3 ToUnityTypeOrDefault(ArenaServices.Vector3 v, Vector3 d)
    {
        if (v == null)
            return d;
        return ToUnityType(v);
    }

    public static Quaternion ToUnityType(ArenaServices.Quaternion v)
    {
        return new Quaternion
        {
            x = v.X,
            y = v.Y,
            z = v.Z,
            w = v.W
        };
    }

    public static Quaternion ToUnityTypeOrDefault(ArenaServices.Quaternion v, Quaternion d)
    {
        if (v == null)
            return d;
        return ToUnityType(v);
    }
}
