using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArenaServices;

namespace MainServer.Arenas
{
    public interface IArena : IDisposable
    {
        void AddLeftPlayer(Player player);
        void AddRightPlayer(Player player);
        void RemoveLeftPlayer(Player player);
        void RemoveRightPlayer(Player player);
        void FillPlayerDecl(Player player, EventArena_PlayerDecl decl);
    }
}
