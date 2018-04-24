using System;
using System.Collections.Generic;
using System.Text;

namespace ArenaHost
{
    internal struct Config
    {
        internal int MaxArenaCount;
        internal bool ShowArenaWindows;
        internal int ArenaStartUpDelaySec;
    }

    internal interface IConfigurable
    {
        void SetConfig(Config config);
    }

    internal class AllConfigurables : IConfigurable
    {
        List<IConfigurable> configurables = new List<IConfigurable>();

        public void SetConfig(Config config)
        {
            foreach (var c in configurables)
                c.SetConfig(config);
        }

        public void Add(IConfigurable c)
        {
            configurables.Add(c);
        }
    }
}
