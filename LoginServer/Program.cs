using Grpc.Core;
using ArenaHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Backend = new ConsoleLogger();

            var instance_server_credentials = new List<InstanceServerDecl>();
            instance_server_credentials.Add(new InstanceServerDecl
            {
                login = new InstanceServerLogin
                {
                    app_id = Guid.Empty.ToString(),
                    secret = Guid.Empty.ToString()
                },
                name = "Test Server",
                url = "localhost:" + PortConstants.MainServerPort
            });

            try
            {
                using (var instance_servers = InstanceServers.Create())
                {
                    var auth = new PlayerAuthDev();
                    var player_store = new PlayerStore();
                    var public_service_impl = new PublicLoginServiceImpl(auth, player_store, instance_servers);
                    var private_service_impl = new PrivateLoginServiceImpl(instance_servers, player_store);

                    public_service_impl.SetServerCredentials(instance_server_credentials);
                    instance_servers.SetServerCredentials(instance_server_credentials);



                    var server = new Server
                    {
                        Services =
                        {
                            LoginServices.LoginPrivateService.BindService(private_service_impl),
                            LoginServices.LoginPublicService.BindService(public_service_impl)
                        },
                        Ports = { new ServerPort("localhost", PortConstants.LoginServerPort, ServerCredentials.Insecure) } //TODO: SSL is required here. ALWAYS
                    };

                    Log.Msg("Starting server...");
                    server.Start();
                    Log.Msg(string.Format("Listening on port '{0}'", PortConstants.LoginServerPort));
                    Log.Msg("Press any key to stop the server...");
                    Console.ReadKey();

                    Log.Msg("Shutting down...");
                    server.ShutdownAsync().Wait();
                    Log.Msg("Shut down server...");
                }
            } catch(Exception e)
            {
                Log.Exception(e);
            }            
        }
    }
}
