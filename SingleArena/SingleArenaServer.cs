using Grpc.Core;
using System;
using System.Threading;

namespace ArenaHost
{
    public class SingleArenaServer : IDisposable
    {
        Server server;
        SingleArenaServicePrivateImpl service;

        public SingleArenaServicePrivateImpl.RemoveCB OnRemovePlayer;

        internal SingleArenaServer(short port)
        {
            service = new SingleArenaServicePrivateImpl();
            service.OnRemovePlayer += on_remove_player;
            server = new Server
            {
                Services =
                {
                    ArenaServer.ArenaServicePrivate.BindService(service)
                },
                Ports =
                {
                    new ServerPort("localhost", port, ServerCredentials.Insecure) //TODO: SSL
                }
            };
            server.Start();
        }

        private void on_remove_player(long player_permanent_id)
        {
            OnRemovePlayer(player_permanent_id);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    service.Shutdown();
                    Thread.Sleep(1000);
                    server.ShutdownAsync().Wait();
                    service.OnRemovePlayer -= on_remove_player;
                }

                disposedValue = true;
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
