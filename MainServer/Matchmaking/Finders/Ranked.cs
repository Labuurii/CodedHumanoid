using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainServer.Matchmaking.Finders
{
    /// <summary>
    /// This is a big TODO.
    /// </summary>
    internal class Ranked : MatchFinderBase
    {
        const float START_RATING_OFFSET = 100;
        const float RATING_OFFSET_STEP_SIZE = 100;
        const double RATING_OFFSET_DELTA_TIME = 30;

        GetSkillLevelDG get_skill_level_cb;
        SetSkillLevelDG set_skill_level_cb;

        readonly SortedList<float, PlayerInfo> players = new SortedList<float, PlayerInfo>();
        readonly ConcurrentQueue<PlayerInfo> add_queue = new ConcurrentQueue<PlayerInfo>();

        struct PlayerInfo
        {
            internal Player player;
            internal CyclicTimer rating_offset_timer;
            internal float rating_offset;
        }

        internal delegate float GetSkillLevelDG(Player player);
        internal delegate void SetSkillLevelDG(Player player, float new_skill_level);

        public Ranked(GetSkillLevelDG get_skill_level_cb, SetSkillLevelDG set_skill_level_cb, int cb)
        {
            this.get_skill_level_cb = get_skill_level_cb;
            this.set_skill_level_cb = set_skill_level_cb;
        }

        public override void AddPlayer(Player player)
        {
            add_queue.Enqueue(new PlayerInfo
            {
                player = player,
                rating_offset_timer = new CyclicTimer(RATING_OFFSET_DELTA_TIME),
                rating_offset = START_RATING_OFFSET
            });
        }

        public override void Run()
        {
            //Update players
            {
                PlayerInfo info;
                while (add_queue.TryDequeue(out info))
                {
                    var skill = get_skill_level_cb(info.player);
                    players.Add(skill, info);
                }
            }

            //Try match
            {
                var skills = players.Keys;
                var all_info = players.Values;
                for (var i = 0; i < all_info.Count; ++i)
                {
                    var target_skill = skills[i];
                    var info = all_info[i];

                    //TODO: Teamsize, match skills and try to add them.
                    for(var j = i + 1; j < all_info.Count; ++j)
                    {
                        var match_skill = skills[i];
                    }
                }
            }
        }
    }
}
