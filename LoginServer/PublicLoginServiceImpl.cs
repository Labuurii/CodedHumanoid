using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.Collections;
using Grpc.Core;
using LoginServices;
using ArenaHost;

namespace LoginServer
{
    internal class PublicLoginServiceImpl : LoginPublicService.LoginPublicServiceBase, ISetServerCredentials
    {
        static readonly Event login_failed = new Event
        {
            LoginResult = new EventLoginResult
            {
                Success = false
            }
        };

        IPlayerAuth auth;
        PlayerStore player_store;
        InstanceServers instance_servers;
        List<LoginServices.Server> server_credentials = new List<LoginServices.Server>();


        public PublicLoginServiceImpl(IPlayerAuth auth, PlayerStore player_store, InstanceServers instance_servers)
        {
            this.auth = auth;
            this.player_store = player_store;
            this.instance_servers = instance_servers;
        }

        public override async Task ConnectAndLogIn(LogInAttempt request, IServerStreamWriter<Event> responseStream, ServerCallContext context)
        {
            {
                var name = request.Username;
                var pw = request.Password;
                if(string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pw))
                {
                    await responseStream.WriteAsync(login_failed);
                    return;
                }

                Guid permanent_id;
                if(!await auth.GetPermanentId(name, pw, out permanent_id))
                {
                    await responseStream.WriteAsync(login_failed);
                    return;
                }

                var out_stream = MTNetOutStream<Event>.Create(context, responseStream);
                if(!await player_store.AddPlayer(permanent_id, out_stream))
                {
                    await responseStream.WriteAsync(login_failed);
                    return;
                }

                {
                    var ev = new Event
                    {
                        LoginResult = new EventLoginResult
                        {
                            Success = true,
                            Token = permanent_id.ToString()
                        }
                    };
                    ev.LoginResult.Servers.AddRange(server_credentials);
                    await responseStream.WriteAsync(ev);
                }

                for(; ; )
                {
                    try
                    {
                        if (!await out_stream.SendCurrentEvents())
                            break;
                        await Task.Delay(100);
                    }
                    catch (InvalidOperationException)
                    {
                        //Happens when cancellation token is set to true
                        break;
                    }
                    catch (RpcException)
                    {
                        break; //This is expected to happen when user logs out
                    } catch (Exception e)
                    {
                        Log.Exception(e);
                        break;
                    }
                    
                }

                out_stream.Nullify();
                await PlayerStore.RemovePlayerAndTellInstanceServer(player_store, permanent_id, instance_servers);
            }
        }

        public void SetServerCredentials(List<InstanceServerDecl> credentials)
        {
            //Sets the cached login_successful message
            var servers = new List<LoginServices.Server>(credentials.Count);
            foreach (var decl in credentials)
                servers.Add(new LoginServices.Server
                {
                    Name = decl.name,
                    Url = decl.url
                });
            server_credentials = servers;
        }
    }
}
