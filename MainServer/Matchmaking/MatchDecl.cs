using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainServer.Matchmaking
{
    /// <summary>
    /// Reference semantics.
    /// </summary>
    public struct MatchDecl
    {
        /// <summary>
        /// These never changes in size and primitive types are thread safe so this is thread safe.
        /// </summary>
        List<Player> left, right;
        bool[] left_accepted, right_accepted;
        
        /// <summary>
        /// The lists are consumed.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public MatchDecl(List<Player> left, List<Player> right)
        {
            this.left = left ?? throw new ArgumentNullException("left");
            this.right = right ?? throw new ArgumentNullException("right");
            left_accepted = new bool[left.Count];
            right_accepted = new bool[right.Count];
        }

        public void Nullify()
        {
            left = null;
            right = null;
        }

        public List<Player> Left
        {
            get
            {
                return left;
            }
        }

        internal void TimeOut()
        {
            //TODO: Try send queue timed out to all players.
            throw new NotImplementedException();
        }

        public List<Player> Right
        {
            get
            {
                return right;
            }
        }

        public enum AcceptInviteResult
        {
            Success,
            AllPlayersIsReady
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ArgumentException">If player is not part of match declaration.</exception>
        /// <param name="player"></param>
        /// <returns></returns>
        public AcceptInviteResult AcceptInvite(Player player)
        {
            if (!try_set_accepted_invite(player, left, left_accepted))
                if (!try_set_accepted_invite(player, right, right_accepted))
                    throw new ArgumentException(string.Format("Player '{0}' is not part of the match declaration", player));

            if (AllHasAccepted())
                return AcceptInviteResult.AllPlayersIsReady;
            return AcceptInviteResult.Success;
        }

        internal bool AllHasAccepted()
        {
            return !left_accepted.Contains(false)
                && !right_accepted.Contains(false);
        }

        private static bool try_set_accepted_invite(Player player, List<Player> list, bool[] accepted_list)
        {
            var idx = list.IndexOf(player);
            if (idx == -1)
                return false;
            accepted_list[idx] = true;
            return true;
        }
    }
}
