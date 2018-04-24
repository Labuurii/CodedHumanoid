using Jitter.LinearMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainServer
{
    internal static class MatrixOps
    {
        internal static bool CmpExact(JMatrix a, JMatrix b)
        {
            return a.M11 == b.M11
                && a.M12 == b.M12
                && a.M13 == b.M13
                && a.M21 == b.M21
                && a.M22 == b.M22
                && a.M23 == b.M23
                && a.M31 == b.M31
                && a.M32 == b.M32
                && a.M33 == b.M33;
        }
    }
}
