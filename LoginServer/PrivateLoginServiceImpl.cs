using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using LoginServices;
using ArenaHost;

namespace LoginServer
{
    

    internal class PrivateLoginServiceImpl : LoginPrivateService.LoginPrivateServiceBase
    {
        InstanceServers instance_servers;
        PlayerStore player_store;

        static readonly PrivateEvent login_failure = new PrivateEvent
        {
            LoginResult = new LoginResult
            {
                Success = false
            }
        };

        static readonly Event server_dc = new Event
        {
            ServerDisconnected = new ServerDisconnected
            {

            }
        };

        static readonly TakeOwnershipResult take_ownership_invalid_player_id = new TakeOwnershipResult
        {
            Result = TakeOwnershipResult.Types.Result.InvalidPlayerId
        };

        static readonly TakeOwnershipResult take_ownership_not_auth = new TakeOwnershipResult
        {
            Result = TakeOwnershipResult.Types.Result.NotAuth
        };

        static readonly TakeOwnershipResult take_ownership_already_has_owner = new TakeOwnershipResult
        {
            Result = TakeOwnershipResult.Types.Result.AlreadyHasOwner
        };

        static readonly TakeOwnershipResult take_ownership_already_owned_by_server = new TakeOwnershipResult
        {
            Result = TakeOwnershipResult.Types.Result.AlreadyOwnedByServer
        };

        static readonly TakeOwnershipResult take_ownership_success = new TakeOwnershipResult
        {
            Result = TakeOwnershipResult.Types.Result.Success
        };


        static readonly LeaveOwnershipResult leave_ownership_invalid_player_id = new LeaveOwnershipResult
        {
            Result = LeaveOwnershipResult.Types.Result.InvalidPlayerId
        };

        static readonly LeaveOwnershipResult leave_ownership_not_auth = new LeaveOwnershipResult
        {
            Result = LeaveOwnershipResult.Types.Result.NotAuth
        };

        static readonly LeaveOwnershipResult leave_ownership_not_owned = new LeaveOwnershipResult
        {
            Result = LeaveOwnershipResult.Types.Result.NoOwner
        };

        static readonly LeaveOwnershipResult leave_ownership_success = new LeaveOwnershipResult
        {
            Result = LeaveOwnershipResult.Types.Result.Success
        };

        public PrivateLoginServiceImpl(InstanceServers instance_servers, PlayerStore player_store)
        {
            this.instance_servers = instance_servers;
            this.player_store = player_store;
        }

        public override async Task<LeaveOwnershipResult> LeaveOwnership(LeaveOwnershipAttempt request, ServerCallContext context)
        {
            Guid server_id;
            if (instance_servers.GetServerId(context, out server_id))
                return leave_ownership_not_auth;

            Guid player_id;
            if (!Guid.TryParse(request.PlayerPermanentId, out player_id))
                return leave_ownership_invalid_player_id;

            var res = await player_store.ReleaseOwnerOfPlayer(server_id, player_id);
            switch (res)
            {
                case ReleasePlayerOwnerResult.NotOwned:
                    return leave_ownership_not_owned;
                case ReleasePlayerOwnerResult.Success:
                    return leave_ownership_success;
                default:
                    throw new Exception("Unhandled enum value " + res);
            }
        }

        public async override Task<TakeOwnershipResult> TakeOwnership(TakeOwnershipAttempt request, ServerCallContext context)
        {
            Guid server_id;
            if (instance_servers.GetServerId(context, out server_id))
                return take_ownership_not_auth;

            Guid player_id;
            if (!Guid.TryParse(request.PlayerPermanentId, out player_id))
                return take_ownership_invalid_player_id;
            var res = await player_store.SetOwnerOfPlayer(server_id, player_id);

            switch(res)
            {
                case SetOwnerResult.PlayerAlreadyHasOwner:
                    return take_ownership_already_has_owner;
                case SetOwnerResult.PlayerAlreadyOwnedByServer:
                    return take_ownership_already_owned_by_server;
                case SetOwnerResult.ServerNotRegistered:
                    return take_ownership_not_auth;
                case SetOwnerResult.Success:
                    return take_ownership_success;
                default:
                    throw new Exception("Unhandled enum value " + res);
            }
        }

        public override async Task Connect(ConnectionAttempt request, IServerStreamWriter<PrivateEvent> responseStream, ServerCallContext context)
        {
            if (string.IsNullOrEmpty(request.Secret) || string.IsNullOrEmpty(request.AppId))
            {
                await responseStream.WriteAsync(login_failure);
                return;
            }

            var login_result_nullable = instance_servers.AddServer(new InstanceServerLogin
            {
                app_id = request.AppId,
                secret = request.Secret
            }, responseStream, context);
            if(!login_result_nullable.HasValue)
            {
                await responseStream.WriteAsync(login_failure);
                return;
            }

            var login_result = login_result_nullable.Value;
            if(!await player_store.AddServer(login_result.guid))
            {
                instance_servers.RemoveServer(login_result.guid);
                await responseStream.WriteAsync(login_failure);
                return;
            }

            try
            {
                await responseStream.WriteAsync(new PrivateEvent
                {
                    LoginResult = new LoginResult
                    {
                        Success = true,
                        Token = login_result.guid.ToString()
                    }
                });

                for (; ; )
                {
                    if (!await login_result.stream.SendCurrentEvents())
                        break;
                    await Task.Delay(100);
                }

            }
            catch (InvalidOperationException)
            {
                //Happens when cancellation token is set to true
            }
            catch (RpcException)
            {
                //Expected to happen if main server goes down.
            } catch(Exception e)
            {
                Log.Exception(e);
            } finally
            {
                login_result.stream.Nullify();
                Debug.Assert(instance_servers.RemoveServer(login_result.guid));
                var players_on_server = await player_store.RemoveServer(login_result.guid);
                if (players_on_server != null)
                {
                    foreach (var player in players_on_server)
                    {
                        var player_permanent_id = player.Key;
                        var stream = player.Value.player_stream;
                        stream.Enqueue(server_dc);
                    }
                }
            }
        }
    }
}
