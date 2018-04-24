using ArenaServices;
using Grpc.Core;
using MainServer.Matchmaking;
using ArenaHost;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainServer
{
    public class Player
    {
        /// <summary>
        /// This should be used by player containers so offline player objects are dropped gracefully.
        /// </summary>
        internal volatile bool online;
        internal readonly Guid permanent_id;
        internal Guid session_id; //This has to be initialized before the player is added to the PlayerAuth internal container.
        internal readonly MTNetOutStream<Event3D> stream_3d = new MTNetOutStream<Event3D>(null, null);
        internal readonly MTNetOutStream<QueueStateMsg> stream_match_maker = new MTNetOutStream<QueueStateMsg>(null, null);
        internal readonly MTNetOutStream<EventArena> stream_arena = new MTNetOutStream<EventArena>(null, null);
        internal readonly MTNetOutStream<EventBase> stream_base = new MTNetOutStream<EventBase>(null, null); 
        internal readonly MatchMaker match_maker = MatchMaker.Create();

        internal readonly string DisplayName;

        /// <summary>
        /// Lazily instantiated.
        /// </summary>
        private ArenaServices.Player decl;

        public Player()
        {
            permanent_id = Guid.NewGuid();
        }

        public Player(string display_name) : base()
        {
            DisplayName = display_name;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Player;
            if(other != null)
            {
                return permanent_id.Equals(other.permanent_id);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return permanent_id.GetHashCode();
        }

        public override string ToString()
        {
            return permanent_id.ToString();
        }


        internal ArenaServices.Player GetDecl()
        {
            if(decl == null)
            {
                decl = new ArenaServices.Player
                {
                    Language = "",
                    Name = DisplayName
                };
            }

            return decl;
        }
    }
}
