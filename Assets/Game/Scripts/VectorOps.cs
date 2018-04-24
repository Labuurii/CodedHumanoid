using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class VectorOps
{
    public static Vector3 Mul(Vector3 a, Vector3 b)
    {
        a.x *= b.x;
        a.y *= b.y;
        a.z *= b.z;
        return a;
    }
}
