using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using ArenaHost;

namespace MainServer
{
    /// <summary>
    /// Multithreaded.
    /// </summary>
    internal class PlayerAuth
    {
        const string SessionTokenHeader = "pl_session_token";

        /// <summary>
        /// This list is added to first. Then <see cref="permanent_id_mapped"/>.
        /// Removal is defined to happen in the reverse order.
        /// </summary>
        readonly ConcurrentDictionary<Guid, Player> session_id_mapped = new ConcurrentDictionary<Guid, Player>();
        readonly ConcurrentDictionary<Guid, Player> permanent_id_mapped = new ConcurrentDictionary<Guid, Player>();
        readonly LoginServerClientLL login_server;
        readonly IDB db;

        internal static PlayerAuth Instance;

        internal PlayerAuth(LoginServerClientLL login_server, IDB db)
        {
            this.login_server = login_server;
            this.db = db;

            login_server.OnDisconnected += on_disconnected_from_server;
            login_server.OnPlayerDisconnected += on_player_disconnected;
        }

        private void on_player_disconnected(Guid permanent_id)
        {
            Player player;
            if (permanent_id_mapped.TryGetValue(permanent_id, out player))
            {
                LogOutPlayer(player);
            }
        }

        private void on_disconnected_from_server()
        {
            throw new NotImplementedException(); //TODO: Shut down server
        }

        internal async Task<KeyValuePair<Guid, Player>?> TakeOwnerShipOfPlayer(Guid token)
        {
            try
            {
                var res = await login_server.Client.TakeOwnershipAsync(new LoginServices.TakeOwnershipAttempt
                {
                    PlayerPermanentId = token.ToString()
                }, login_server.call_metadata);
                switch (res.Result)
                {
                    case LoginServices.TakeOwnershipResult.Types.Result.AlreadyHasOwner:
                        break; //Normal failure condition
                    case LoginServices.TakeOwnershipResult.Types.Result.InvalidPlayerId:
                        Log.Fatal("Sent invalid player id to login server. Atleast that is what the server tells us.");
                        break;
                    case LoginServices.TakeOwnershipResult.Types.Result.NotAuth:
                        Log.Fatal("Main Server does not have access to the login server. Shutting down...");
                        break; //TODO: Shut down server

                    case LoginServices.TakeOwnershipResult.Types.Result.Success:
                        return await get_player_from_db_and_add_to_local_storage(token);
                    case LoginServices.TakeOwnershipResult.Types.Result.AlreadyOwnedByServer:
                        Log.Warning("Tried to take ownership of player we already have ownership of. Recovers from error.");
                        //If player is not in the list this means a programming bug. But it is recoverable.
                        return await get_player_from_db_and_add_to_local_storage(token);
                    default:
                        Log.Fatal("Unhandled enum value " + res.Result);
                        break;
                }
            } catch(RpcException)
            {
                Log.Fatal("Login server seems to be down. So lets shut down...");
                //TODO: Shut down server
            } catch(Exception e)
            {
                Log.Exception(e);
                //TODO: Shut down server
            }

            return null;
        }

        private async Task<KeyValuePair<Guid, Player>?> get_player_from_db_and_add_to_local_storage(Guid token)
        {
            var player = await db.GetPlayer(token);
            if (player != null)
            {
                player.online = true;
                if(!permanent_id_mapped.TryAdd(player.permanent_id, player))
                {
                    Log.Warning("Does not expect user to be logged in here. Since this should only be called when the login server has given us ownership.");
                    return null;
                }

                Guid session_id;
                for (; ; )
                {
                    session_id = Guid.NewGuid();
                    player.session_id = session_id;
                    if (session_id_mapped.TryAdd(session_id, player))
                        break;
                }

                return new KeyValuePair<Guid, Player>(session_id, player);
            }
            else
            {
                Log.Fatal("Could not get player from database even though we managed to take ownership of the actual player. In this case the database should throw an exception and then the whole server infrastructure should go down.");
            }

            return null;
        }

        internal void LogOutPlayer(Player player)
        {
            player.online = false;
            if (!permanent_id_mapped.TryRemove(player.permanent_id, out player) || !session_id_mapped.TryRemove(player.session_id, out player))
                return;

            Task.Run(async () =>
            {
                var res = await login_server.Client.LeaveOwnershipAsync(new LoginServices.LeaveOwnershipAttempt
                {
                    PlayerPermanentId = player.permanent_id.ToString()
                }, login_server.call_metadata);

                switch(res.Result)
                {
                    case LoginServices.LeaveOwnershipResult.Types.Result.InvalidPlayerId:
                        Log.Fatal("Sent an invalid UUID to server. Atleast that is what the login server says...");
                        break;
                    case LoginServices.LeaveOwnershipResult.Types.Result.NoOwner:
                        Log.Fatal("Tried to log out player which does not have any owner.");
                        break;
                    case LoginServices.LeaveOwnershipResult.Types.Result.NotAuth:
                        break; //TODO: Shut down server. No longer authenticated with login server.
                    case LoginServices.LeaveOwnershipResult.Types.Result.Success:
                        break; //Nothing to do
                    default:
                        Log.Fatal("Unhandled enum value " + res.Result);
                        throw new Exception("Unhandled enum value " + res.Result);
                }
            });
        }

        internal Player GetPlayer(ServerCallContext context)
        {
            foreach(var entry in context.RequestHeaders)
            {
                if(entry.Key == SessionTokenHeader)
                {
                    Guid session_id;
                    if (Guid.TryParse(entry.Value, out session_id))
                    {
                        Player player;
                        if(session_id_mapped.TryGetValue(session_id, out player))
                        {
                            return player;
                        }
                    }
                }
            }

            return null;
        }
    }
}
