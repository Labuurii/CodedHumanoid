using Grpc.Core;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class ClientBaseLL : IDisposable {

    protected readonly Channel channel;

    readonly List<CancellationTokenSource> cancel_sources = new List<CancellationTokenSource>();
    readonly ConcurrentQueue<Action> main_thread_queue = new ConcurrentQueue<Action>();

    public delegate void OnDisconnectedCB(bool internal_error, StatusCode? status);
    public OnDisconnectedCB OnDisconnected;

    public ClientBaseLL(Channel channel)
    {
        this.channel = channel;
    }

    protected void OnMainThread(Action a)
    {
        MainThreadEventQueue.Enqueue(a);
    }

    protected CancellationTokenSource HandleEventsAsync<T>(AsyncServerStreamingCall<T> call, Func<T, Task> handler)
    {
        var cancel_source = new CancellationTokenSource();
        cancel_sources.Add(cancel_source);
        Task.Run(async () =>
        {
            try
            {
                using (call)
                {
                    var stream = call.ResponseStream;
                    while (await stream.MoveNext(cancel_source.Token))
                    {
                        var task = handler(stream.Current);
                        if (task != null)
                            await task;
                    }
                }
            }
            catch (RpcException e)
            {
                Debug.LogError("Got RpcException: " + e.Message);
                OnMainThread(() => OnDisconnected(false, e.Status.StatusCode));
                return;

            }
            catch (Exception e)
            {
                Debug.LogError("Got unexpected subscription exception: " + e.Message);
                OnMainThread(() => OnDisconnected(true, null));
                return;
            }

            Debug.Log("Stopped handle subscriptions");
        });
        return cancel_source;
    }

    #region IDisposable Support
    private bool disposedValue = false; // To detect redundant calls

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                foreach (var cancel_source in cancel_sources)
                    cancel_source.Cancel();
                cancel_sources.Clear();
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
