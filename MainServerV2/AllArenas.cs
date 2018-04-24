using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ArenaHost;
using ArenaServer;
using ServerClientSharedV2;

namespace MainServerV2
{
    internal class AllArenas : IConfigurable, IDisposable
    {
        ThreadSafeSinglyLinkedList<ArenaHost> hosts = new ThreadSafeSinglyLinkedList<ArenaHost>();

        internal void TryCreateArenaForPlayersAsync(IMatchMaker match_maker, List<Player> buffer)
        {
            if (is_disposing())
                return;

            Task.Run(async () =>
            {
                var attempt = new CreateArenaAttempt();
                foreach(var player in buffer)
                {
                    attempt.PlayerInfo.Add(new ArenaServer.PlayerInfo
                    {
                        AuthToken = GuidOps.ToByteString(Guid.NewGuid()),
                        BasicInfo = player.BasicPlayerInfo()
                    });
                }

                foreach(var arena in hosts)
                {
                    if (await arena.TryCreateArenaForPlayers(buffer, attempt))
                        return;
                }

                match_maker.AddPlayers(buffer);
            });
        }

        public void SetConfig(Config config)
        {
            if (is_disposing())
                return;
            foreach (var host in config.ArenaHosts)
            {
                if (!hosts.Contains((a, b) => a.ConnectionString == b, host))
                {
                    hosts.Add(new ArenaHost(host));
                }
            }

            foreach(var host in hosts)
            {
                host.SetConfig(config);
            }
        }

        bool is_disposing([CallerMemberName] string method = null)
        {
            if(disposedValue)
            {
                Log.Warning(string.Format("Tried to call method {0} while disposing", method));
                return true;
            }
            return false;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                disposedValue = true;
                if (disposing)
                {
                    foreach(var host in hosts)
                    {
                        host.Dispose();
                    }
                }
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
