using LoginServices;
using ArenaHost;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LoginServer
{
    internal enum SetOwnerResult
    {
        Success,
        ServerNotRegistered,
        PlayerAlreadyHasOwner,
        PlayerAlreadyOwnedByServer
    }
    
    internal enum ReleasePlayerOwnerResult
    {
        Success,
        NotOwned
    }

    internal struct PlayerData
    {
        internal Guid? owning_server_id;
        internal MTNetOutStream<Event> player_stream;
    }

    /// <summary>
    /// Multithreaded.
    /// </summary>
    internal class PlayerStore
    {
        readonly ConcurrentQueue<Task> tasks;
        readonly HashSet<Guid> servers;
        /// <summary>
        /// Key == player, Value == server.
        /// </summary>
        readonly Dictionary<Guid, PlayerData> players_server_mapping;
        readonly Thread worker;

        internal PlayerStore()
        {
            tasks = new ConcurrentQueue<Task>();
            servers = new HashSet<Guid>();
            players_server_mapping = new Dictionary<Guid, PlayerData>();
            worker = new Thread(run_tasks_forever);
            worker.IsBackground = true;
            worker.Name = "PlayerStore worker";
            worker.Start();
        }

        private void run_tasks_forever()
        {
            try
            {
                for (; ; )
                {
                    Task task;
                    while (tasks.TryDequeue(out task))
                    {
                        task.RunSynchronously();
                    }
                    Thread.Sleep(1);
                }
            } catch(Exception e)
            {
                Log.Exception(e);
            }
        }

        public Task<bool> AddServer(Guid guid)
        {
            var t = new Task<bool>(() =>
            {
                return servers.Add(guid);
            });
            tasks.Enqueue(t);
            return t;
        }

        /// <summary>
        /// Returns a list of the removed players. Is null if server is not here.
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        public Task<List<KeyValuePair<Guid, PlayerData>>> RemoveServer(Guid server)
        {
            var t = new Task<List<KeyValuePair<Guid, PlayerData>>>(() =>
            {
                if (!servers.Remove(server))
                    return null;
                var removed_players = new List<KeyValuePair<Guid, PlayerData>>();
                foreach(var kv in players_server_mapping)
                {
                    if(kv.Value.owning_server_id.HasValue & kv.Value.owning_server_id == server)
                    {
                        removed_players.Add(kv);
                    }
                }
                foreach (var player in removed_players)
                    Debug.Assert(players_server_mapping.Remove(player.Key));
                return removed_players;
            });
            tasks.Enqueue(t);
            return t;
        }

        public Task<bool> AddPlayer(Guid permanent_id, MTNetOutStream<Event> out_stream)
        {
            var t = new Task<bool>(() =>
            {
                try
                {
                    players_server_mapping.Add(permanent_id, new PlayerData
                    {
                        player_stream = out_stream
                    });
                    return true;
#pragma warning disable CS0168 // Variable is declared but never used
                }
                catch (ArgumentException already_in_container_e)
#pragma warning restore CS0168 // Variable is declared but never used
                {
                    return false;
                }
                
            });
            tasks.Enqueue(t);
            return t;
        }

        /// <summary>
        /// Returns the owning server.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public Task<PlayerData> RemovePlayer(Guid player)
        {
            var t = new Task<PlayerData>(() =>
            {
                PlayerData player_data;
                if (players_server_mapping.TryGetValue(player, out player_data))
                {
                    Debug.Assert(players_server_mapping.Remove(player));
                }
                return player_data;
            });
            tasks.Enqueue(t);
            return t;
        }

        public Task<SetOwnerResult> SetOwnerOfPlayer(Guid server, Guid player)
        {
            var t = new Task<SetOwnerResult>(() =>
            {
                PlayerData current_player_data;
                if (players_server_mapping.TryGetValue(player, out current_player_data))
                {
                    if(current_player_data.owning_server_id.HasValue)
                    {
                        return current_player_data.owning_server_id.Value == server ? SetOwnerResult.PlayerAlreadyOwnedByServer : SetOwnerResult.PlayerAlreadyHasOwner;
                    }

                    //Success condition
                    current_player_data.owning_server_id = server;
                    players_server_mapping.Add(player, current_player_data);
                    return SetOwnerResult.Success;
                }
                return SetOwnerResult.ServerNotRegistered;
            });
            tasks.Enqueue(t);
            return t;
        }

        public Task<ReleasePlayerOwnerResult> ReleaseOwnerOfPlayer(Guid owner_server, Guid player)
        {
            var t = new Task<ReleasePlayerOwnerResult>(() =>
            {
                PlayerData current_player_data;
                if (players_server_mapping.TryGetValue(player, out current_player_data))
                {
                    if(!current_player_data.owning_server_id.HasValue)
                    {
                        return ReleasePlayerOwnerResult.NotOwned;
                    }

                    current_player_data.owning_server_id = null;
                    players_server_mapping.Add(player, current_player_data);
                    return ReleasePlayerOwnerResult.Success;
                }
                return ReleasePlayerOwnerResult.NotOwned;
            });
            tasks.Enqueue(t);
            return t;
        }

        public static async Task RemovePlayerAndTellInstanceServer(PlayerStore player_store, Guid player_permanent_id, InstanceServers instance_servers)
        {
            var res = await player_store.RemovePlayer(player_permanent_id);
            if (res.owning_server_id.HasValue)
            {
                var server_stream = instance_servers.GetServerStream(res.owning_server_id.Value);
                if (server_stream == null)
                    return;
                server_stream.Enqueue(new PrivateEvent
                {
                    PlayerDisconnected = new PlayerDisconnected
                    {
                        PermanentId = player_permanent_id.ToString()
                    }
                });
            }
        }
    }
}
