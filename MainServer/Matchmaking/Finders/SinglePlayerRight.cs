using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainServer.Matchmaking.Finders
{
    internal class SinglePlayerRight : MatchFinderBase
    {
        public override void AddPlayer(Player player)
        {
            var left = new List<Player>();
            var right = new List<Player>(1);
            right.Add(player);
            PopQueueForPlayers(left, right);
        }

        public override void Run()
        {
            //Nothing to do
        }
    }
}
