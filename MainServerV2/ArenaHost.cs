using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ArenaHost;
using ArenaServer;
using Google.Protobuf;
using Grpc.Core;
using static ArenaServer.ArenaHostServicePrivate;

namespace MainServerV2
{
    internal class ArenaHost : IDisposable, IConfigurable
    {
        string connection_str;
        Channel channel;
        ArenaHostServicePrivateClient client;
        int suggested_arena_instances_left = 500;
        List<SingleArena> arenas = new List<SingleArena>();
        int delta_between_reconnect_attempt_sec;

        public string ConnectionString {
            get
            {
                return connection_str;
            }
        }

        internal ArenaHost(string connection_str)
        {
            this.connection_str = connection_str;
            channel = new Channel(connection_str, ChannelCredentials.Insecure); //TODO: SSL, Compression
            client = new ArenaHostServicePrivateClient(channel);

            try_connect();
        }

        internal async Task<bool> TryCreateArenaForPlayers(List<Player> buffer, CreateArenaAttempt attempt)
        {
            if (is_disposing())
                return false;

            if (channel.State != ChannelState.Ready)
                return false;

            if (buffer.Count != attempt.PlayerInfo.Count)
            {
                Log.Fatal("Parameter buffer is expected to have same size and attempt.PlayerInfo");
                return false;
            }

            if(suggested_arena_instances_left == 0)
            {
                return false;
            }

            CreateArenaResult result;

            try
            {
                result = await client.CreateArenaAsync(attempt);
                if (!result.Success)
                    return false;

            } catch(RpcException)
            {
                Log.Msg(string.Format("Could not speak to arena host with connection string: {0}", connection_str));
                try_connect();
                return false;
            } catch (Exception e)
            {
                Log.Exception(e);
                return false;
            }
            
            suggested_arena_instances_left = result.FreeArenaCount;

            var arena = new SingleArena(this, buffer, result);
            lock (arenas)
                arenas.Add(arena);

            for(var i = 0; i < buffer.Count; ++i)
            {
                var player = buffer[i];
                var info = attempt.PlayerInfo[i];

                player.arena = arena;
                player.arena_state = ArenaState.InArena;

                var ev = new Event
                {
                    QueuePopped = new Event_QueuePopped
                    {
                        AuthToken = info.AuthToken,
                        ConnectionStr = result.PublicConnectionStr
                    }
                };

                player.event_stream.Enqueue(ev);
            }

            return true;
        }

        private void try_connect()
        {
            Log.Msg("Tries to connect to arena host with connection string: " + connection_str);
            Task.Run(async () =>
            {
                for(;;)
                {
                    try
                    {
                        await channel.ConnectAsync(DateTime.Now + TimeSpan.FromSeconds(5));
                        break;
                    } catch(RpcException)
                    {
                        //Expected
                    } catch(Exception e)
                    {
                        Log.Exception(e);
                    }

                    await Task.Delay(delta_between_reconnect_attempt_sec * 1000);
                }
            });
        }

        internal void ArenaShutdown(SingleArena arena)
        {
            if (is_disposing())
                return;

            bool did_remove;
            lock (arenas)
                did_remove = arenas.Remove(arena);
            if(!did_remove)
            {
                Log.Warning("ArenaShutdown was with an arena which is not part of this host.");
                return;
            }

            ++suggested_arena_instances_left;
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
                    foreach (var arena in arenas)
                        arena.Dispose();
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

        public void SetConfig(Config config)
        {
            delta_between_reconnect_attempt_sec = config.ArenaHostReconnectDeltaSec;
        }
    }
}
