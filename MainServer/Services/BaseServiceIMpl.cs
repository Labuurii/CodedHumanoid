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
    internal class BaseServiceImpl : BaseService.BaseServiceBase
    {
        public override async Task Subscribe(BaseServerSubscriptionAttempt request, IServerStreamWriter<EventBase> responseStream, ServerCallContext context)
        {
            Guid token;
            if (!Guid.TryParse(request.Token, out token))
                return;

            var player_and_session = await PlayerAuth.Instance.TakeOwnerShipOfPlayer(token);
            if (!player_and_session.HasValue)
                return;

            var session_id = player_and_session.Value.Key;
            var player = player_and_session.Value.Value;
            if (player == null)
                return;

            try
            {
                if (!player.stream_base.SetStream(responseStream, context))
                    return;

                await responseStream.WriteAsync(new EventBase
                {
                    SubscriptionSuccessful = new EventBase_SubscriptionSuccessful
                    {
                        SessionToken = session_id.ToString()
                    }
                });

                for (; ; )
                {
                    if (!player.online || !await player.stream_base.SendCurrentEvents())
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
                //Nothing really exceptional
            }
            catch (Exception e)
            {
                Log.Exception(e);
            }
            finally
            {
                PlayerAuth.Instance.LogOutPlayer(player);
            }
        }
    }
}
