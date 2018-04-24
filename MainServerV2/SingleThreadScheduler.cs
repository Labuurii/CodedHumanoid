using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArenaHost
{
    internal class SingleThreadScheduler : TaskScheduler, IDisposable
    {
        readonly Thread thread;
        readonly CancellationTokenSource cancel_source = new CancellationTokenSource();
        readonly BlockingCollection<Task> tasks = new BlockingCollection<Task>();

        internal SingleThreadScheduler(string name)
        {
            thread.Name = name;
            thread = new Thread(run);
            thread.Start();
        }

        internal Task Schedule(Action a)
        {
            return Task.Factory.StartNew(a, cancel_source.Token, TaskCreationOptions.None, this);
        }

        internal Task<T> Schedule<T>(Func<T> a)
        {
            return Task.Factory.StartNew(a, cancel_source.Token, TaskCreationOptions.None, this);
        }

        private void run()
        {
            var cancel_token = cancel_source.Token;

            try
            {
                foreach (var task in tasks.GetConsumingEnumerable())
                {
                    try
                    {
                        TryExecuteTask(task);
                    }
                    catch (Exception e)
                    {
                        Log.Exception(e);
                    }
                }
            } catch(OperationCanceledException)
            {
                //Should happen on destruction
            }
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return null;
        }

        protected override void QueueTask(Task task)
        {
            try
            {
                tasks.Add(task, cancel_source.Token);
            }
            catch (OperationCanceledException)
            { }
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            if (taskWasPreviouslyQueued || !disposedValue)
                return false;
            return TryExecuteTask(task);
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    cancel_source.Cancel();
                    thread.Join();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
