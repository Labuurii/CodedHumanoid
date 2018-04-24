using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArenaHost;
using ArenaServer;
using Grpc.Core;
using static ArenaServer.ArenaServicePrivate;

namespace MainServerV2
{
    internal class SingleArena : IDisposable
    {
        ArenaHost arena_host;
        List<Player> players;
        Channel channel;
        ArenaServicePrivateClient client;
        CancellationTokenSource cancel_source = new CancellationTokenSource();

        public SingleArena(ArenaHost arena_host, List<Player> buffer, CreateArenaResult result)
        {
            this.arena_host = arena_host;
            players = buffer;
            channel = new Channel(result.PrivateConnectionStr, ChannelCredentials.Insecure); //TODO: SSL, Compression, OAuth
            client = new ArenaServicePrivateClient(channel);

            Task.Run(subscribe_and_handle);
        }

        internal void RemovePlayerAsync(Player player)
        {
            if (is_disposing())
                return;

            remove_player(player.permanent_id);
            Task.Run(async () => {
                var attempt = new RemovePlayerAttempt
                {
                    PlayerId = player.permanent_id
                };

                try
                {
                    var result = await client.RemovePlayerAsync(attempt);
                    if (!result.IsRemoved)
                    {
                        Log.Warning("Tried to remove player from service but the service is not running.");
                    }
                } catch(RpcException)
                {
                    //Server is down. There is nothing to do
                } catch (Exception e)
                {
                    Log.Exception(e);
                }
            });
        }

        private async Task subscribe_and_handle()
        {
            var call = client.Subscribe(new ArenaServer.SubscriptionAttempt());
            using (call)
            {
                var stream = call.ResponseStream;

                while (await stream.MoveNext(cancel_source.Token))
                {
                    var ev = stream.Current;
                    switch (ev.EventCase)
                    {
                        case Event_Arena.EventOneofCase.ArenaShutdown:
                            Dispose();
                            break;
                        case Event_Arena.EventOneofCase.PlayerLeft:
                            {
                                var permanent_id = ev.PlayerLeft.PlayerId;
                                remove_player(permanent_id);
                            }
                            break;
                        case Event_Arena.EventOneofCase.None:
                            Log.Warning("Got a none event from a single arena");
                            break;
                        default:
                            throw new Exception("Unhandled enum value " + ev.EventCase);
                    }
                }
            }
        }

        private void remove_player(long permanent_id)
        {
            lock(players)
            {
                for (var i = 0; i < players.Count; ++i)
                {
                    if (players[i].permanent_id == permanent_id)
                    {
                        players[i].RemoveFromArena();
                        players.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        bool is_disposing([CallerMemberName] string method = null)
        {
            if (disposedValue)
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
                if (disposing)
                {
                    cancel_source.Cancel();
                    arena_host.ArenaShutdown(this);
                    lock(players)
                    {
                        foreach(var player in players)
                        {
                            player.RemoveFromArena();
                        }
                    }
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            try
            {
                Dispose(true);
            } catch(Exception e)
            {
                Log.Exception(e);
            }
            
        }
        #endregion
    }
}
