using CommandLine;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArenaHost
{
    class Program
    {
        public class Options
        {
            [Option(Required = true, HelpText = "The command to run the arena server.")]
            public string unity_file { get; set; }

            [Option(Required = true, HelpText = "The url to this server.")]
            public string self_url { get; set; }

            [Option(Required = true, HelpText = "The minimum port number which the single arenas will use for communication with the clients.")]
            public ushort port_base { get; set; }

            [Option(Required = true, HelpText = "The maximum port number which the single arenas will use for communication with the clients.")]
            public ushort port_top { get; set; }

            [Option(Required = true, HelpText = "The port which the arena manager (host) uses for communication with the main server.")]
            public ushort host_port { get; set; }

            internal static void ThrowErrorMsg(IEnumerable<Error> errors)
            {
                var sb = new StringBuilder();
                sb.Append("Command line args is invalid because: \n");
                foreach (var error in errors)
                {
                    sb.AppendLine(error.ToString());
                }
                throw new Exception(sb.ToString());
            }
        }


        static void Main(string[] args)
        {
            Options options = null;
            {
                var parse_result = Parser.Default.ParseArguments<Options>(args);
                parse_result
                    .WithParsed(opts => options = opts)
                    .WithNotParsed((errors) => Options.ThrowErrorMsg(errors));
            }

            Log.Backend = new ConsoleLogger();

            var port_allocator = PortAllocator.Create(options.port_base, options.port_top);
            var service = new ArenaHostPrivateServiceImpl(options.unity_file, options.self_url, port_allocator);
            var server = new Server
            {
                Services =
                {
                    ArenaServer.ArenaHostServicePrivate.BindService(service)
                },
                Ports =
                {
                    new ServerPort("localhost", options.host_port, ServerCredentials.Insecure)
                }
            };
            server.Start();

            Log.Msg("Server is now running...");
            Log.Msg("Press any key to stop the server.");

            Console.ReadKey();
            Log.Msg("Stopping server...");
            server.ShutdownAsync().Wait();
            Log.Msg("Successfully stopped server...");
            Log.Msg("Remember the arenas owned by this server can still be running.");
        }
    }
}
