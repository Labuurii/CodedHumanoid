using ArenaServices;
using Jitter;
using Jitter.Collision;
using Jitter.Dynamics;
using Jitter.LinearMath;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainServer.Arenas
{
    internal class Agar : Arena3D
    {
        enum StaticObjects
        { }

        enum SpawnableObjects
        {
            Ring = 0
        }

        RigidBody obj;

        internal Agar()
            : base(new CollisionSystemBrute())
        {}

        internal override void Run(float delta_time)
        {
            if(obj == null)
            {
                left_players.Access((list) =>
                {
                    if (list.Count > 0)
                    {
                        this.obj = SpawnObject((int)SpawnableObjects.Ring, new JVector(1, 0, 0)).Value;
                    }
                });
            }
        }

        RigidBody GetObject(StaticObjects obj)
        {
            RigidBody rb = rb = GetObject((int)obj);
            return rb;
        }
    }
}
