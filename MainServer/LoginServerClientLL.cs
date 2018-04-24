using Grpc.Core;
using LoginServices;
using ArenaHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MainServer
{
    /// <summary>
    /// Expects to already be logged in.
    /// </summary>
    internal class LoginServerClientLL : IDisposable
    {
        readonly Channel channel;
        readonly LoginPrivateService.LoginPrivateServiceClient client;
        readonly CancellationTokenSource cancel_token = new CancellationTokenSource();

        internal readonly Metadata call_metadata;

        public delegate void DisconnectedCB();
        public DisconnectedCB OnDisconnected;

        public delegate void PlayerDisconnectedCB(Guid permanent_id);
        public PlayerDisconnectedCB OnPlayerDisconnected;

        public LoginServerClientLL(
            Channel channel,
            LoginPrivateService.LoginPrivateServiceClient client,
            AsyncServerStreamingCall<PrivateEvent> stream,
            string token)
        {
            this.channel = channel;
            this.client = client;
            call_metadata = new Metadata();
            call_metadata.Add(HeaderConstants.PrivateServiceTokenHeader, token);
            Task.Run(() => handle_stream(stream));
        }

        internal LoginPrivateService.LoginPrivateServiceClient Client
        {
            get
            {
                return client;
            }
        }

        private async Task handle_stream(AsyncServerStreamingCall<PrivateEvent> call)
        {
            try
            {
                using (call)
                {
                    var stream = call.ResponseStream;
                    while (await stream.MoveNext(cancel_token.Token))
                    {
                        var ev = stream.Current;
                        switch (ev.EventCase)
                        {
                            case PrivateEvent.EventOneofCase.LoginResult:
                            case PrivateEvent.EventOneofCase.None:
                            case PrivateEvent.EventOneofCase.PlayerDisconnected:
                            default:
                                Log.Fatal("Unhandled enum value " + ev.EventCase);
                                break;
                        }
                    }
                }
            } catch(RpcException)
            {
                Log.Warning("Login server shut down. So we are shutting down...");
            } catch(Exception e)
            {
                Log.Exception(e);
                Log.Fatal("Shutting down because of exception...");
            } finally
            {
                Dispose();
            }
            
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    cancel_token.Cancel();
                    OnDisconnected();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
