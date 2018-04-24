using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainServer
{
    interface IMatchFinder
    {
        void AddPlayer(Player player);
        void Run();
    }
}
