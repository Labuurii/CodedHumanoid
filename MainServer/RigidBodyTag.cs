using Jitter.LinearMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainServer
{
    internal class RigidBodyTag
    {
        internal readonly int object_id;

        internal RigidBodyTag(int object_id)
        {
            this.object_id = object_id;
        }
    }
}
