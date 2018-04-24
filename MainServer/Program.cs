using ArenaServices;
using Grpc.Core;
using Jitter;
using Jitter.Collision;
using Jitter.Collision.Shapes;
using Jitter.Dynamics;
using Jitter.LinearMath;
using MainServer.Arenas;
using MainServer.Matchmaking;
using MainServer.Matchmaking.Finders;
using MainServer.Services;
using ArenaHost;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Jitter.Collision.Shapes.CompoundShape;

namespace MainServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Backend = new ConsoleLogger();

            Log.Msg("Connecting to DB");
            var db = new DebugDB();
            Log.Success("Connected to DB!");

            Log.Msg("Connecting to login server...");
            {
                var channel = new Channel("localhost:" + PortConstants.LoginServerPort, ChannelCredentials.Insecure); //TODO: Port? //TODO: SSL
                var private_client = new LoginServices.LoginPrivateService.LoginPrivateServiceClient(channel);
                var call = private_client.Connect(new LoginServices.ConnectionAttempt
                {
                    AppId = Guid.Empty.ToString(),
                    Secret = Guid.Empty.ToString()
                });

                var stream = call.ResponseStream;
                if (!stream.MoveNext().Result)
                    throw new Exception("Could not connect to login server.");
                var ev = stream.Current;
                switch (ev.EventCase)
                {
                    case LoginServices.PrivateEvent.EventOneofCase.LoginResult:
                        {
                            var l = ev.LoginResult;
                            if (!l.Success)
                            {
                                throw new Exception("Could not log in to server.");
                            }

                            var security_token = l.Token;
                            var login_client = new LoginServerClientLL(channel, private_client, call, security_token);
                            PlayerAuth.Instance = new PlayerAuth(login_client, db);
                            Log.Success("Connected to login server!");
                            }
                        break;
                    default:
                        throw new Exception("Expected login result as first event from login server.");
                }
            }


            Log.Msg("Setting up the rest of the server...");
            var all_match_finders = new List<IMatchFinder>();
            var arenas = new List<IArenaFactory>();
            MatchEngineLookUp.Prepare(
                create_arena, 
                create_match_finder_for_arena_type_skirmish,
                ref all_match_finders,
                ref arenas);
            MatchEngineLookUp.Prepare(
                create_arena,
                create_match_finder_for_arena_type_ranked,
                ref all_match_finders,
                ref arenas);

            var match_engine_lookup = new MatchEngineLookUp(all_match_finders.ToArray(), arenas.ToArray());

            SmallWorkScheduler.Instance.SetMatchFinders(all_match_finders);

            var arena_data_bin = File.ReadAllBytes(@"./scene_data.bin"); //TODO: Actual dir
            var arena_data = Arenadata.ArenaData.Parser.ParseFrom(arena_data_bin);
            var collision_data = ArenaDataConv.ToCollisionData(arena_data);

            var arena = new Agar();
            arena.SetStartupRigidbodies(collision_data.static_.Concat(collision_data.dynamic));
            arena.SetSpawnableRigidbodies(collision_data.spawnable);

            var service_3d = new ArenaService3DImpl(arena);
            var service_mm = new MatchMakerServiceImpl(match_engine_lookup);
            var service_arena_base = new ArenaBaseServiceImpl();
            var service_base = new BaseServiceImpl();

            var server = new Server
            {
                Services =
                {
                    Arena3DService.BindService(service_3d),
                    MatchMakerService.BindService(service_mm),
                    ArenaBaseService.BindService(service_arena_base),
                    BaseService.BindService(service_base)
                },
                Ports = { new ServerPort("localhost", PortConstants.MainServerPort, ServerCredentials.Insecure) } //TODO: Compression
            };

            Log.Msg("Starting server...");
            server.Start();
            Log.Msg(string.Format("Listening on port '{0}'", PortConstants.MainServerPort));
            Log.Msg("Press any key to stop the server...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }

        private static IMatchFinder create_match_finder_for_arena_type_skirmish(Arena arena, int team_size)
        {
            switch(team_size)
            {
                case 1:
                case 2:
                case 3:
                    return new Skirmish(team_size);
                default:
                    throw new Exception("Unhandled team size: " + team_size);
            }
        }

        private static IMatchFinder create_match_finder_for_arena_type_ranked(Arena arena, int team_size)
        {
            switch (team_size)
            {
                case 1:
                case 2:
                case 3:
                    return new Ranked((p) => 0, (p, skill) => { }, team_size); //TODO
                default:
                    throw new Exception("Unhandled team size: " + team_size);
            }
        }

        private static IArenaFactory create_arena(Arena arena)
        {
            switch(arena)
            {
                case Arena.Agar:
                    return null; //TODO:
                case Arena.None:
                    throw new ArgumentException("Did not expect " + arena);
                default:
                    throw new Exception("Unhandled enum value " + arena);
            }
        }
    }
}
