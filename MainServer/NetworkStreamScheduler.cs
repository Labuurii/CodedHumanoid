using Grpc.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MainServer
{
    /// <summary>
    /// Makes sure all networked schedulers run in the same thread.
    /// </summary>
    /*
    static class NetworkStreamExecutor
    {
        static readonly Thread thread = new Thread(run_all);
        internal static readonly WorkList<Action> schedulers = WorkList<Action>.Create(8);

        internal static void Start()
        {
            if (thread.IsAlive)
                throw new Exception("Start has already been called.");
            thread.IsBackground = true;
            thread.Start();
        }

        private static void run_all(object state)
        {
            Debug.Assert(state == null, "Not used");
            schedulers.Update();
            foreach (var a in schedulers.List)
                a();
        }
    }


    internal class NetworkStreamScheduler<T>
    {
        readonly ConcurrentQueue<Work> work = new ConcurrentQueue<Work>();
        internal static readonly NetworkStreamScheduler<T> Instance = new NetworkStreamScheduler<T>();

        struct Work
        {
            internal Player player;
            internal IServerStreamWriter<T> stream;
            internal T ev;
        }

        struct SendTask
        {
            internal IServerStreamWriter<T> stream;
            internal Task task;
            internal Player player;

            internal void Nullify()
            {
                stream = null;
                task = null;
                player = null;
            }

            internal bool IsNull()
            {
                return stream == null;
            }
        }

        private NetworkStreamScheduler() {
            NetworkStreamExecutor.schedulers.Add(send_events);
        }

        void send_events()
        {
            //Try send as many events as possible
            {
                Work instruction;
                while (work.TryDequeue(out instruction))
                {
                    instruction.stream.WriteAsync(instruction.ev).Wait();
                }
            }
        }

        public void Enqueue(IServerStreamWriter<T> stream, T ev, Player player)
        {
            if (stream == null)
                return;

            work.Enqueue(new Work
            {
                player = player,
                stream = stream,
                ev = ev
            });
        }
    }
    */
}
