using MainServerV2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaHost
{
    enum ArenaState
    {
        NotQueued,
        Queued,
        InArena
    }

    internal class LinearMatchMaker : IMatchMaker, IConfigurable, IDisposable
    {
        int min_players_in_arena;
        int delta_sec_between_pop_tries;
        Queue<Player> queue = new Queue<Player>();
        AllArenas all_arenas;

        public LinearMatchMaker(AllArenas all_arenas)
        {
            this.all_arenas = all_arenas ?? throw new ArgumentNullException(nameof(all_arenas));
        }

        public void AddPlayer(Player player)
        {
            if (player == null)
            {
                Log.Warning("Got a null player.");
            }
            player.arena_state = ArenaState.Queued;
            queue.Enqueue(player);
        }

        public void RemovePlayer(Player player)
        {
            if (player == null)
            {
                Log.Warning("Got a null player.");
                return;
            }

            //Super slow and with the worst scaling of all time!!
            var count = queue.Count;
            for (var i = 0; i < count; ++i)
            {
                var queued_player = queue.Dequeue();
                if (queued_player == player)
                {
                    player.arena_state = ArenaState.NotQueued;
                    break;
                }
                queue.Enqueue(queued_player);
            }
        }

        public void RemovePlayers(List<Player> players)
        {
            foreach (var p in players)
                RemovePlayer(p);
        }

        public void AddPlayers(List<Player> players)
        {
            foreach(var player in players)
            {
                if(player == null)
                {
                    Log.Warning("Got a null player.");
                    continue;
                }
                player.arena_state = ArenaState.Queued;
                queue.Enqueue(player);
            }
        }

        internal void Run(SingleThreadScheduler scheduler)
        {
            scheduler.Schedule(run_impl);
        }

        private async Task run_impl()
        {
            for(;;)
            {
                if (disposedValue)
                    break;

                try
                {
                    if (queue.Count >= min_players_in_arena)
                    {
                        var buffer = new List<Player>(min_players_in_arena);
                        for(var i = 0; i < min_players_in_arena; ++i)
                        {
                            var player = queue.Dequeue();
                            if (!player.online)
                                continue;
                            buffer.Add(player);
                        }

                        if(buffer.Count > 0)
                        {
                            all_arenas.TryCreateArenaForPlayersAsync(this, buffer);
                        }
                    }
                } catch(Exception e)
                {
                    Log.Exception(e);
                }
                await Task.Delay(delta_sec_between_pop_tries * 1000);
            }
        }

        public void SetConfig(Config config)
        {
            min_players_in_arena = config.MinPlayersInArena;
            delta_sec_between_pop_tries = config.MatchMakingPulseDeltaSec;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    //Nothing to do
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
