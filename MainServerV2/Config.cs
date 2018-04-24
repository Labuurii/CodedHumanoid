using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaHost
{
    internal struct Config
    {
        internal string GameVersion;

        internal int MaxFriendSize;
        internal int MaxFriendRequestSize;
        internal int MaxGroupSize;
        internal int GroupInviteTimeoutSec;
        internal int MinPlayersInArena;
        internal string[] ArenaHosts;
        internal int MatchMakingPulseDeltaSec;
        internal int MaxMessageLength;
        internal int MaxPlayersOnline;
        internal int ArenaHostReconnectDeltaSec;
    }

    internal interface IConfigurable
    {
        /// <summary>
        /// Multithreaded but not reentrant.
        /// </summary>
        void SetConfig(Config config);
    }

    internal class AllConfigurables : IConfigurable
    {
        internal List<IConfigurable> list = new List<IConfigurable>();

        public void SetConfig(Config config)
        {
            foreach (var c in list)
                c.SetConfig(config);
        }

        internal void Add(IConfigurable c)
        {
            list.Add(c);
        }
    }
}
