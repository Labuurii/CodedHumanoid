using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArenaServices;

namespace MainServer.Arenas
{
    public abstract class ArenaBase : IArena
    {
        protected readonly AtomicData<List<Player>> left_players, right_players;

        public abstract void Dispose();

        public ArenaBase()
        {
            left_players = new AtomicData<List<Player>>(new List<Player>(2));
            right_players = new AtomicData<List<Player>>(new List<Player>(2));
        }

        /// <summary>
        /// When overriding: Make sure to always call this base first before doing anything else!
        /// </summary>
        /// <param name="player"></param>
        public virtual void AddLeftPlayer(Player player)
        {
            left_players.Access((list) =>
            {
                list.Add(player);
            });
        }

        /// <summary>
        /// When overriding: Make sure to always call this base first before doing anything else!
        /// </summary>
        /// <param name="player"></param>
        public virtual void AddRightPlayer(Player player)
        {
            right_players.Access((list) =>
            {
                list.Add(player);
            });
        }

        /// <summary>
        /// When overriding: Make sure to always call this base first before doing anything else!
        /// </summary>
        /// <param name="player"></param>
        public virtual void RemoveLeftPlayer(Player player)
        {
            left_players.Access((list) =>
            {
                list.Remove(player);
                //TODO: Send to other players about player being removed.
            });
        }

        /// <summary>
        /// When overriding: Make sure to always call this base first before doing anything else!
        /// </summary>
        /// <param name="player"></param>
        public virtual void RemoveRightPlayer(Player player)
        {
            right_players.Access((list) =>
            {
                list.Remove(player);
                //TODO: Send to other players about player being removed.
            });
        }

        public virtual void FillPlayerDecl(Player player, EventArena_PlayerDecl decl)
        {
            left_players.Access((players) =>
            {
                foreach(var other_player in players)
                {
                    if(player == other_player)
                    {
                        decl.Left = true;
                    } else
                    {
                        decl.LeftPlayers.Add(other_player.GetDecl());
                    }
                }
            });

            right_players.Access((players) =>
            {
                foreach(var other_player in players)
                {
                    if(player != other_player)
                    {
                        decl.RightPlayers.Add(other_player.GetDecl());
                    }
                }
            });
        }
    }
}
