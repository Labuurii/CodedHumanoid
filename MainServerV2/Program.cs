using Grpc.Core;
using MainServerV2;
using ServerClientSharedV2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Backend = new ConsoleLogger();
            var all_configurables = new AllConfigurables();

            var config = new Config
            {
                GroupInviteTimeoutSec = 30,
                MaxFriendRequestSize = 200,
                MaxFriendSize = 500,
                MaxGroupSize = 5,
                MatchMakingPulseDeltaSec = 15,
                MinPlayersInArena = 1,
                MaxMessageLength = 1024,
                MaxPlayersOnline = 1000,
                ArenaHostReconnectDeltaSec = 5,
                ArenaHosts = new[] { "localhost:19999" }
            };

            using (var in_memory_worker = new SingleThreadScheduler("In Memory Request Worker"))
            using(var db = new DB("server=localhost;uid=root;pwd=secret;database=pl_war"))
            using (var player_auth = new PlayerAuth(db, in_memory_worker))
            using (var all_arenas = new AllArenas())
            using(var match_maker = new LinearMatchMaker(all_arenas))
            {
                var main_service_handler = new MainServiceImpl(player_auth, db, in_memory_worker, match_maker);
                all_configurables.Add(main_service_handler);
                all_configurables.Add(all_arenas);
                all_configurables.SetConfig(config);

                match_maker.Run(in_memory_worker);

                var server = new Server
                {
                    Services =
                    {
                        MainServiceV2.BindService(main_service_handler)
                    },
                    Ports = { new ServerPort("localhost", Constants.MAIN_SERVER_PORT, ServerCredentials.Insecure) } //TODO: Compression, SSL
                };
                server.Start();
                Log.Msg("Listening on port " + Constants.MAIN_SERVER_PORT);
                Log.Msg("Enter any button in command line to stop service.");
                Console.ReadKey();

                server.ShutdownAsync().Wait();
            }
        }
    }
}
