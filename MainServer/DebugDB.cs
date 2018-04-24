using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainServer
{
    internal class DebugDB : IDB
    {
        public Task<Player> GetPlayer(Guid token)
        {
            return Task.FromResult(new Player("Kickupx"));
        }
    }
}
