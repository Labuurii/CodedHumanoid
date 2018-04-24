using ArenaServices;
using Grpc.Core;
using ArenaHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainServer.Services
{
    internal class ArenaBaseServiceImpl : ArenaBaseService.ArenaBaseServiceBase
    {
        public override async Task Subscribe(SubscriptionAttempt request, IServerStreamWriter<EventArena> responseStream, ServerCallContext context)
        {
            var player = PlayerAuth.Instance.GetPlayer(context);
            if (player == null)
                return;

            if (player.match_maker.GetState() != Matchmaking.MatchMaker.State.InArena)
                return;

            //Send the involved players
            {
                var arena = player.match_maker.GetArena();
                if (arena == null)
                    return;

                if (!player.stream_arena.SetStream(responseStream, context))
                    return;

                var ev = new EventArena
                {
                    PlayerDecl = new EventArena_PlayerDecl()
                };
                arena.FillPlayerDecl(player, ev.PlayerDecl);
                await responseStream.WriteAsync(ev);
            }

            try
            {
                for (; ; )
                {
                    if (!player.online || !await player.stream_arena.SendCurrentEvents())
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
                //Expected to happen when player leaves arena
            }
            catch (Exception e)
            {
                Log.Exception(e);
            }
            finally
            {
                remove_player_from_arena(player);
            }
        }

        private static void remove_player_from_arena(Player player)
        {
            var arena = player.match_maker.GetArena();
            if (arena != null)
            {
                arena.RemoveLeftPlayer(player);
                arena.RemoveRightPlayer(player);
            }
        }
    }
}
