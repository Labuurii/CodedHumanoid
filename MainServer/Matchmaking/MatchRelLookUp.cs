using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArenaServices;
using MainServer.Arenas;
using ServerClientShared;

namespace MainServer.Matchmaking
{
    internal struct MatchEngineLookUp
    {
        static readonly Arena[] arena_enum_values = (Arena[])Enum.GetValues(typeof(Arena));
        static readonly TeamSize[] team_size_enum_values = (TeamSize[])Enum.GetValues(typeof(TeamSize));

        static readonly int MaxMatchModeValue = Enum.GetNames(typeof(MatchMode)).Length;
        static readonly int MaxTeamSizeValue = Enum.GetNames(typeof(TeamSize)).Length;
        static readonly int ArenaTypesCount = Enum.GetNames(typeof(Arena)).Length - 1; //Arena.None is not counted.

        readonly IMatchFinder[] match_finders;
        readonly IArenaFactory[] arena_factories;

        public MatchEngineLookUp(IMatchFinder[] match_finders, IArenaFactory[] arena_factories)
        {
            this.match_finders = match_finders;
            this.arena_factories = arena_factories;
        }

        internal static void Prepare(Func<Arena, IArenaFactory> factory_factory, Func<Arena, int, IMatchFinder> match_finder_factory, ref List<IMatchFinder> match_finders_sink, ref List<IArenaFactory> arena_sink) //LOL, don't care.
        {
            for (var i = 0; i < ArenaTypesCount; ++i)
            {
                var arena = arena_enum_values[i + 1];
                for (var j = 0; j < MaxTeamSizeValue; ++j)
                {
                    var team_size = team_size_enum_values[j];
                    IArenaFactory factory = null;
                    IMatchFinder match_finder = null;
                    if (ArenaOps.IsMatchSizeSupported(arena, team_size))
                    {
                        factory = factory_factory(arena);
                        match_finder = match_finder_factory(arena, j + 1);
                    }
                    match_finders_sink.Add(match_finder);
                    arena_sink.Add(factory); //The team size as human number
                }
            }
        }

        internal IMatchFinder GetMatchFinder(MatchMode mode, Arena arena, TeamSize size)
        {
            if (arena == Arena.None) //The zero case
                throw new ArgumentException("The parameter 'arena' can not be Arena.None");
            int idx = calc_idx(mode, arena, size);
            return match_finders[idx];
        }

        internal IArenaFactory GetArenaFactory(MatchMode mode, Arena arena, TeamSize size)
        {
            if (arena == Arena.None) //The zero case
                throw new ArgumentException("The parameter 'arena' can not be Arena.None");
            var idx = calc_idx(mode, arena, size);
            return arena_factories[idx];
        }

        private static int calc_idx(MatchMode mode, Arena arena, TeamSize size)
        {
            var mode_int = (int)mode;
            var arena_int = (int)arena - 1;
            var team_size_int = (int)size;
            var idx = mode_int * MaxMatchModeValue * MaxTeamSizeValue + arena_int * MaxTeamSizeValue + team_size_int;
            return idx;
        }
    }
}
