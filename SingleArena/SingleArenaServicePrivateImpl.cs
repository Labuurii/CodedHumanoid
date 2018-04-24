using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ArenaServer;
using Grpc.Core;

namespace ArenaHost
{
    public class SingleArenaServicePrivateImpl : ArenaServicePrivate.ArenaServicePrivateBase
    {
        MTNetOutStream<Event_Arena> connected_main_server = MTNetOutStream<Event_Arena>.Create();

        public delegate void RemoveCB(long player_permanent_id);

        public RemoveCB OnRemovePlayer;

        public override async Task Subscribe(SubscriptionAttempt request, IServerStreamWriter<Event_Arena> responseStream, ServerCallContext context)
        {
            if (!connected_main_server.SetStream(responseStream, context))
                return;

            try
            {
                for (; ; )
                {
                    if (!await connected_main_server.SendCurrentEvents())
                        break;
                    await Task.Delay(5);
                }
            }
            catch(InvalidOperationException) { }
            catch (RpcException) {}
            catch(Exception e)
            {
                Log.Exception(e);
            }
        }

        public override Task<RemovePlayerResult> RemovePlayer(RemovePlayerAttempt request, ServerCallContext context)
        {
            try
            {
                var player_id = request.PlayerId;
                OnRemovePlayer(player_id);
                return Task.FromResult(new RemovePlayerResult
                {
                    IsRemoved = true
                });
            } catch(Exception e)
            {
                Log.Exception(e);
                return Task.FromResult(new RemovePlayerResult
                {
                    IsRemoved = false
                });
            }
        }

        public void PlayerLeft(long player_permanent_id)
        {
            var ev = new Event_Arena
            {
                PlayerLeft = new EventArena_PlayerLeft
                {
                    PlayerId = player_permanent_id
                }
            };
            connected_main_server.Enqueue(ev);
        }

        public void Shutdown()
        {
            var ev = new Event_Arena
            {
                ArenaShutdown = new EventArena_Shutdown()
            };
            connected_main_server.Enqueue(ev);
        }
    }
}
