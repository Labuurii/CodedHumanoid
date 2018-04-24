using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArenaServer;
using Grpc.Core;
using static ArenaServer.ArenaHostServicePrivate;

namespace ArenaHost
{
    internal class ArenaHostPrivateServiceImpl : ArenaHostServicePrivateBase, IConfigurable
    {
        string invoke_unity_file;
        string self_url;
        PortAllocator port_allocator;
        ThreadSafeSinglyLinkedList<Process> processes = new ThreadSafeSinglyLinkedList<Process>();

        int max_arena_count;
        bool show_arena_windows;
        int process_startup_delay_sec;

        internal ArenaHostPrivateServiceImpl(string invoke_unity_file, string self_url, PortAllocator port_allocator)
        {
            this.invoke_unity_file = invoke_unity_file ?? throw new ArgumentNullException(nameof(invoke_unity_file));
            this.self_url = self_url ?? throw new ArgumentNullException(nameof(self_url));
            this.port_allocator = port_allocator;
        }

        public override async Task<CreateArenaResult> CreateArena(CreateArenaAttempt request, ServerCallContext context)
        {
            var result = new CreateArenaResult();

            var args = new StringBuilder();

            args.Append("-executeMethod SingleArenaServerController.Run ");

            var private_port = port_allocator.Allocate();
            if(!private_port.HasValue)
            {
                Log.Warning("Could not allocate a port from the port allocator");
                result.Success = false;
                return result;
            }

            var public_port = port_allocator.Allocate();
            if(!public_port.HasValue)
            {
                Log.Warning("Could not allocate a port from the port allocator");
                result.Success = false;
                return result;
            }

            args.Append("-private_port=");
            args.Append(private_port.Value);
            args.Append(" ");
            args.Append("-public_port=");
            args.Append(public_port.Value);
            args.Append(" ");

            if(!show_arena_windows)
            {
                args.Append("-batchmode -nographics ");
            }

            var process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = invoke_unity_file,
                Arguments = args.ToString()
            };

            if (!processes.AddIfCountIsLessThan(process, max_arena_count))
            {
                result.Success = false;
                return result;
            }

            process.EnableRaisingEvents = true;
            process.Exited += on_process_exited;
            process.Start();
            await Task.Delay(process_startup_delay_sec * 1000);
            if(process.HasExited)
            {
                Log.Warning("Could not start arena process properly");
                result.Success = false;
                return result;
            }

            result.FreeArenaCount = max_arena_count - processes.Count();
            result.PrivateConnectionStr = self_url + ":" + private_port.Value;
            result.PublicConnectionStr = self_url + ":" + public_port.Value;
            return result;
        }

        private void on_process_exited(object sender, EventArgs e)
        {
            processes.Remove((Process)sender);
        }

        public void SetConfig(Config config)
        {
            max_arena_count = config.MaxArenaCount;
            show_arena_windows = config.ShowArenaWindows;
            process_startup_delay_sec = config.ArenaStartUpDelaySec;
        }
    }
}
