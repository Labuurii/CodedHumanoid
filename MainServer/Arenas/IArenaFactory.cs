using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainServer.Arenas
{
    internal interface IArenaFactory
    {
        IArena Construct();
    }
}
