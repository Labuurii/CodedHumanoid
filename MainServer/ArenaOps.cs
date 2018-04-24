using ArenaServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerClientShared
{
    public static class ArenaOps
    {
        public static bool IsMatchSizeSupported(Arena arena, TeamSize team_size)
        {
            switch(arena)
            {
                case Arena.None:
                    return false;
                case Arena.Agar:
                    switch(team_size)
                    {
                        case TeamSize.One:
                        case TeamSize.Two:
                        case TeamSize.Three:
                            return true;
                        default:
                            throw new Exception("Unhandled enum value " + team_size);
                    }
                default:
                    throw new Exception("Unhandled enum value " + arena);
            }
        }
    }
}
