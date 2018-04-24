using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainServer.Matchmaking.Finders
{
    internal class SinglePlayerLeft : MatchFinderBase
    {
        public override void AddPlayer(Player player)
        {
            var left = new List<Player>(1);
            left.Add(player);
            var right = new List<Player>();
            PopQueueForPlayers(left, right);
        }

        public override void Run()
        {
            //Nothing to do
        }
    }
}
