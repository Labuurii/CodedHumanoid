using ArenaServices;
using Jitter.Dynamics;
using Jitter.LinearMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainServer
{
    internal static class ArenaServiceConv
    {
        internal static Vector3 ToArenaData(JVector v)
        {
            return new Vector3
            {
                X = v.X,
                Y = v.Y,
                Z = v.Z,
            };
        }

        internal static Quaternion ToArenaData(JQuaternion v)
        {
            return new Quaternion
            {
                X = v.X,
                Y = v.Y,
                Z = v.Z,
                W = v.W
            };
        }

        internal static Quaternion ToArenaData(JMatrix v)
        {
            var q = JQuaternion.CreateFromMatrix(v);
            return ToArenaData(q);
        }

        internal static Transform ToArenaTransform(RigidBody rb)
        {
            return new Transform
            {
                Position = ToArenaData(rb.Position),
                Velocity = ToArenaData(rb.LinearVelocity),
                Rotation = ToArenaData(rb.Orientation)
            }; //TODO: Angular velocity
        }
    }
}
