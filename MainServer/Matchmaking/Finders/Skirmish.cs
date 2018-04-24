using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainServer.Matchmaking.Finders
{
    internal class Skirmish : MatchFinderBase
    {
        readonly List<Player> reused_list = new List<Player>();
        readonly Random rng = new Random();
        readonly ConcurrentQueue<Player> players = new ConcurrentQueue<Player>();
        readonly int player_count;
        readonly int team_size;

        public Skirmish(int team_size)
        {
            if (team_size < 1)
                throw new ArgumentException("team_size has to have a positive number.");

            this.team_size = team_size;
            player_count = team_size * 2;
        }

        public override void AddPlayer(Player player)
        {
            players.Enqueue(player);
        }

        public override void Run()
        {
            while (players.Count >= player_count)
            {
                Player player;
                for (var i = 0; i < player_count; ++i)
                {
                    Debug.Assert(players.TryDequeue(out player));
                    reused_list.Add(player);
                }
                reused_list.Shuffle(rng);

                var left = new List<Player>(team_size);
                var right = new List<Player>(team_size);
                left.AddRange(reused_list.Take(team_size));
                right.AddRange(reused_list.Skip(team_size).Take(team_size));
                PopQueueForPlayers(left, right);
                reused_list.Clear();
            }
        }
    }
}
