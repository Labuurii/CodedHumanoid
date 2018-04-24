using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArenaServices;
using Grpc.Core;
using MainServer.Arenas;
using ArenaHost;

namespace MainServer.Services
{
    internal class ArenaService3DImpl : Arena3DService.Arena3DServiceBase
    {
        readonly Arena3D arena;

        public ArenaService3DImpl(Arena3D arena)
        {
            this.arena = arena;
        }

        public override async Task Subscribe(SubscriptionAttempt request, IServerStreamWriter<Event3D> responseStream, ServerCallContext context)
        {
            var player = PlayerAuth.Instance.GetPlayer(context);
            if(player == null || !player.stream_3d.SetStream(responseStream, context))
                return;

            try
            {
                for(; ; )
                {
                    if (!player.online || !await player.stream_3d.SendCurrentEvents())
                        break;
                    await Task.Delay(5);
                }
            }
            catch (InvalidOperationException)
            {
                //Happens when cancellation token is set to true
            }
            catch (RpcException)
            {
                //Nothing special
            } catch(Exception e)
            {
                Log.Exception(e);
            }
        }
    }
}
