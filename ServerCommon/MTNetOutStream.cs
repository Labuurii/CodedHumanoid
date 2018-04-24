using Grpc.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArenaHost
{
    /// <summary>
    /// Has reference semantics.
    /// Using the default constructor is undefined behaviour.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MTNetOutStream<T>
    {
        object mutex;
        volatile IServerStreamWriter<T> stream;
        volatile ServerCallContext context;
        readonly ConcurrentQueue<T> queue;

        public MTNetOutStream(ServerCallContext context, IServerStreamWriter<T> s)
        {
            stream = s;
            this.context = context;
            mutex = new object();
            queue = new ConcurrentQueue<T>();
        }

        public static MTNetOutStream<T> Create(ServerCallContext context, IServerStreamWriter<T> s = null)
        {
            return new MTNetOutStream<T>(context, s);
        }

        /// <summary>
        /// Is lock-free.
        /// </summary>
        /// <returns></returns>
        public bool IsNull()
        {
            return stream == null;
        }

        public void Nullify()
        {
            stream = null;
        }

        /// <summary>
        /// Multithreaded.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public bool SetStream(IServerStreamWriter<T> stream, ServerCallContext context)
        {
            lock(mutex)
            {
                if (!IsNull()) //IsNull only checks the stream pointer and therefore does not lock.
                    return false;
                this.stream = stream;
                this.context = context;
                return true;
            }
        }

        /// <summary>
        /// Threadsafe.
        /// </summary>
        /// <param name="ev"></param>
        public bool Enqueue(T ev)
        {
            if (IsNull())
                return false;

            queue.Enqueue(ev);
            return true;
        }

        /// <summary>
        /// Returns whether the stream is alive.
        /// </summary>
        /// <exception cref="RpcException">This expected to happen if the user disconnects.</exception>
        /// <exception cref="Exception">This means an internal error and logging should occur.</exception>
        /// <returns></returns>
        public async Task<bool> SendCurrentEvents()
        {
            //Locking not required because all of these operations are themself lockfree.
            var stream = this.stream;
            var queue = this.queue;
            var context = this.context;
            if (stream == null || queue == null || context == null || context.CancellationToken.IsCancellationRequested)
                return false;

            T ev;
            while (queue.TryDequeue(out ev))
            {
                await stream.WriteAsync(ev);
            }
            return true;
        }
    }
}
