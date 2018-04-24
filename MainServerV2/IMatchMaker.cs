using ArenaHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainServerV2
{
    internal interface IMatchMaker
    {
        void AddPlayer(Player player);
        void AddPlayers(List<Player> players);
        void RemovePlayer(Player player);
        void RemovePlayers(List<Player> players);
    }
}
