using Jitter.LinearMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainServer
{
    internal static class VectorOps
    {
        internal static float Distance(JVector a, JVector b)
        {
            var squared = (a.X - b.X) * (a.X - b.X) +
                     (a.Y - b.Y) * (a.Y - b.Y) +
                     (a.Z - b.Z) * (a.Z - b.Z);
            return (float) Math.Sqrt(squared); //TODO: Rewrite Math.sqrt to only using float?
        }
    }
}
