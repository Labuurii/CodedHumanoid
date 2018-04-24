using MainServerV2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaHost
{
    internal class Group
    {
        internal Player leader;
        internal readonly List<Player> players = new List<Player>(1);
        internal readonly List<Player> invited_to_group = new List<Player>(1);

        internal Group(Player leader, Player other)
        {
            this.leader = leader;
            players.Add(leader);
            invited_to_group.Add(other);

        }

        internal bool UpdatePlayerToMember(Player player)
        {
            var leader = this.leader;
            if(leader == null)
            {
                Log.Fatal("The leader of this group is null!");
                return false;
            }

            if (!invited_to_group.Remove(player))
                return false;

            var ev = construct_answer_ev(player, true);
            leader.event_stream.Enqueue(ev);
            players.Add(player);
            return true;
        }

        internal int TotalPossibleMembers()
        {
            return players.Count + invited_to_group.Count;
        }

        internal bool RemoveQualifiedMember(Player player)
        {
            if(player == leader)
            {
                if(!players.Remove(player))
                {
                    Log.Warning("Leader is not in the player list.");
                    return false;
                }
                player.group = null;
                if(players.Count == 0) //Dismiss group
                {
                    foreach(var invited_player in invited_to_group)
                    {
                        if(invited_player.group != this)
                        {
                            Log.Warning("Invited player is not referenced by player itself.");
                        }
                        invited_player.group = null;
                    }
                    invited_to_group.Clear();
                    return true;
                } else //Select new member
                {
                    var rng = ThreadLocal<Random>.Get();
                    var idx = rng.Next(0, players.Count - 1);
                    leader = players[idx];
                    Event ev = construct_player_left_event(player);
                    send_ev_to_members(ev);
                    return true;
                }
            } else
            {
                if (!players.Remove(player))
                {
                    return false;
                }

                player.group = null;
                var ev = construct_player_left_event(player);
                send_ev_to_members(ev);
                return true;
            }
        }

        internal bool RemoveInvitedMember(Player player)
        {
            if (!invited_to_group.Remove(player))
                return false;
            player.group = null;
            var ev = construct_answer_ev(player, false);
            leader.event_stream.Enqueue(ev);

            if (players.Count == 1 && invited_to_group.Count == 0) //There is only the leader 
            {
                leader.group = null;
            }
            return true;
        }

        private static Event construct_answer_ev(Player player, bool accepted)
        {
            return new Event
            {
                PlayerAnsweredGroupInvite = new Event_PlayerAnsweredGroupInvite
                {
                    Player = player.BasicPlayerInfo(),
                    Accepted = accepted
                }
            };
        }

        private void send_ev_to_members(Event ev)
        {
            foreach (var group_player in players)
            {
                group_player.event_stream.Enqueue(ev);
            }
        }

        private Event construct_player_left_event(Player player)
        {
            var ev = new Event
            {
                PlayerLeftGroup = new Event_PlayerLeftGroup
                {
                    NewLeader = leader.BasicPlayerInfo(),
                    PlayerLeaving = player.BasicPlayerInfo(),
                }
            };

            foreach (var group_player in players)
            {
                if (group_player == leader)
                    continue;
                ev.PlayerLeftGroup.CurrentPlayers.Add(group_player.BasicPlayerInfo());
            }

            return ev;
        }
    }
}
