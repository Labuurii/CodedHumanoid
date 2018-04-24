using Grpc.Core;
using LoginServices;
using ArenaHost;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer
{
    internal struct InstanceServer
    {
        internal MTNetOutStream<PrivateEvent> stream;
        internal InstanceServerLogin login;
    }

    internal struct InstanceServerLogin
    {
        internal string app_id, secret;
    }

    internal struct InstanceServerDecl
    {
        internal InstanceServerLogin login;
        internal string url;
        internal string name;
    }

    /// <summary>
    /// Reference semantics.
    /// </summary>
    internal class InstanceServers : ISetServerCredentials, IDisposable
    {
        readonly ConcurrentDictionary<Guid, InstanceServer> logged_in_servers;
        readonly ConcurrentDictionary<InstanceServerLogin, Guid?> login_mapping;

        private InstanceServers(int dummy)
        {
            logged_in_servers = new ConcurrentDictionary<Guid, InstanceServer>();
            login_mapping = new ConcurrentDictionary<InstanceServerLogin, Guid?>();
        }

        internal static InstanceServers Create()
        {
            return new InstanceServers(42);
        }

        internal struct AddServerResult
        {
            internal MTNetOutStream<PrivateEvent> stream;
            internal Guid guid;
        }

        public AddServerResult? AddServer(InstanceServerLogin login, IServerStreamWriter<PrivateEvent> stream, ServerCallContext context)
        {
            if (disposedValue)
                return null;

            var new_uuid = Guid.NewGuid();
            if(login_mapping.TryUpdate(login, new_uuid, null))
            {
                var instance_data = new InstanceServer
                {
                    stream = new MTNetOutStream<PrivateEvent>(context, stream),
                    login = login
                };
                Debug.Assert(logged_in_servers.TryAdd(new_uuid, instance_data));
                return new AddServerResult
                {
                    stream = instance_data.stream,
                    guid = new_uuid
                };
            }
            return null;
        }

        internal bool GetServerId(ServerCallContext context, out Guid server_id)
        {
            var token = GrpcOps.GetHeaderString(context, HeaderConstants.PrivateServiceTokenHeader);
            if(string.IsNullOrEmpty(token))
            {
                server_id = Guid.Empty;
                return false;
            }

            if(Guid.TryParse(token, out server_id))
            {
                return logged_in_servers.ContainsKey(server_id);
            }
            return false;
        }

        public bool RemoveServer(Guid server_token)
        {
            if (disposedValue)
                return false;

            InstanceServer server_data;
            if(logged_in_servers.TryRemove(server_token, out server_data))
            {
                Debug.Assert(login_mapping.TryUpdate(server_data.login, null, server_token));
                return true;
            }
            return false;
        }

        internal MTNetOutStream<PrivateEvent> GetServerStream(Guid server_id)
        {
            if (disposedValue)
                return null;

            InstanceServer server_data;
            if(logged_in_servers.TryGetValue(server_id, out server_data))
            {
                return server_data.stream;
            }
            return null;
        }

        public void SetServerCredentials(List<InstanceServerDecl> credentials)
        {
            //Removal is NOT allowed, changing logins is NOT allowed

            foreach(var decl in credentials)
            {
                var login = decl.login;
                login_mapping.TryAdd(login, null);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                disposedValue = true;

                if (disposing)
                {
                    foreach(var kv in logged_in_servers)
                    {
                        kv.Value.stream.Nullify();
                    }
                }
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
