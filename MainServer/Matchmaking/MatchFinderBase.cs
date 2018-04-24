using ArenaServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainServer.Matchmaking
{
    internal abstract class MatchFinderBase : IMatchFinder
    {
        protected bool PopQueueForPlayers(List<Player> left, List<Player> right)
        {
            var match_decl = new MatchDecl(left, right);
            foreach(var player in left.Concat(right))
            {
                if(!player.online)
                {
                    cancel_queue(left, right);
                    return false;
                }
                var player_res = player.match_maker.AwaitingAccept(match_decl);

                switch(player_res)
                {
                    case MatchMaker.State.AwaitingAccept:
                        break; //Success
                    default:
                        cancel_queue(left, right);
                        return false;
                }
            }

            var ev = new QueueStateMsg
            {
                Authenticated = true,
                CurrentState = QueueState.QueuePopped
            };

            foreach (var player in left.Concat(right))
            {
                player.stream_match_maker.Enqueue(ev);
            }

            return true;
        }

        private static void cancel_queue(List<Player> left, List<Player> right)
        {
            foreach (var cancelled_player in left.Concat(right))
            {
                cancelled_player.match_maker.Cancel();
                //TODO: Send error msg
            }
        }

        public abstract void AddPlayer(Player player);
        public abstract void Run();
    }
}
