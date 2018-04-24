using ArenaServices;
using MainServer.Arenas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MainServer.Matchmaking
{
    /// <summary>
    /// Value semantics.
    /// TODO: Unit test this
    /// </summary>
    public struct MatchMaker
    {
        readonly object mutex;
        State state;

        /// <summary>
        /// Only when state is <see cref="State.AwaitingOtherPlayers"/>
        /// </summary>
        MatchDecl match_player_decl;

        internal State GetState()
        {
            lock (mutex)
                return state;
        }

        /// <summary>
        /// Only when state is <see cref="State.AwaitingOtherPlayers"/>
        /// </summary>
        MatchMode match_mode;

        internal IArena GetArena()
        {
            lock (mutex)
                return arena;
        }

        /// <summary>
        /// Only when state is <see cref="State.AwaitingOtherPlayers"/>
        /// </summary>
        Arena aren_type;

        /// <summary>
        /// Only when state is <see cref="State.AwaitingOtherPlayers"/>
        /// </summary>
        TeamSize team_size;

        /// <summary>
        /// Only when state is <see cref="State.InArena"/>
        /// </summary>
        IArena arena;

        public enum State
        {
            NotQueued,
            Queued,
            AwaitingAccept,
            AwaitingOtherPlayers,
            InArena
        }

        private MatchMaker(int dummy)
        {
            mutex = new object();
            state = State.NotQueued;
            match_player_decl = new MatchDecl();
            match_mode = MatchMode.Skirmish; //Garbage
            aren_type = Arena.None; //Atleast an invalid value
            team_size = TeamSize.One; //Garbage
            arena = null;
        }

        public static MatchMaker Create()
        {
            return new MatchMaker(42);
        }

        private bool try_change_state(State new_value, State required_value, out State old_sink)
        {
            Debug.Assert(new_value != required_value);
            lock(mutex)
            {
                if(state == required_value)
                {
                    state = new_value;
                    old_sink = required_value;
                    return true;
                } else
                {
                    old_sink = state;
                    return false;
                }
            }
        }

        /// <summary>
        /// If return state == <see cref="State.Queued"/> then the player is safe to add to the match finder.
        /// </summary>
        /// <param name="matchFinder"></param>
        /// <returns></returns>
        public State Queue(MatchMode match_mode, Arena arena, TeamSize team_size)
        {
            
            lock(mutex)
            {
                var old = state;
                if (state != State.NotQueued)
                    return state;
                state = State.Queued;
                this.match_mode = match_mode;
                this.aren_type = arena;
                this.team_size = team_size;
                return State.Queued;
            }
        }

        /// <summary>
        /// If the return key == <see cref="State.AwaitingAccept"/> then the return value is the match finder used for queueing up.
        /// </summary>
        /// <returns></returns>
        public State AwaitingAccept(MatchDecl match)
        {
            State old;
            if(try_change_state(State.AwaitingAccept, State.Queued, out old))
            {
                match_player_decl = match;
                return State.AwaitingAccept;
            }
            return old;
        }

        public struct AnswerPendingResult
        {
            public State player_state;
            public MatchDecl match_decl;
            public bool all_is_ready;

            public MatchMode match_mode;
            public Arena arena;
            public TeamSize team_size;

            public override string ToString()
            {
                return ToStringOps.FieldValues(this);
            }
        }

        public State LeaveArena()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The successful player state values are <see cref="State.AwaitingOtherPlayers"/> and <see cref="State.NotQueued"/>.
        /// Accepting pending and not accepting respective.
        /// In all other states the value are not valid.
        /// This also checks if all players have joined.
        /// </summary>
        /// <param name="accepted"></param>
        /// <returns></returns>
        public AnswerPendingResult AnswerPending(Player player, bool accepted)
        {
            var result = new AnswerPendingResult();

            if(accepted)
            {
                lock(mutex)
                {
                    if(state == State.AwaitingAccept)
                    {
                        state = State.AwaitingOtherPlayers;

                        result.match_decl = match_player_decl;
                        result.player_state = State.AwaitingOtherPlayers;
                        var match_result = match_player_decl.AcceptInvite(player);
                        switch (match_result)
                        {
                            case MatchDecl.AcceptInviteResult.AllPlayersIsReady:
                                result.all_is_ready = true;
                                break;
                            case MatchDecl.AcceptInviteResult.Success:
                                break;
                            default:
                                throw new Exception("Unhandled enum value " + match_result);
                        }
                        match_player_decl.Nullify();
                        return result;
                    } else
                    {
                        result.player_state = state;
                        return result;
                    }
                }
            } else
            {
                lock(mutex)
                {
                    if(state == State.AwaitingAccept)
                    {
                        state = State.NotQueued;
                        result.player_state = State.NotQueued;
                        result.match_decl = match_player_decl;
                        match_player_decl.Nullify();
                        return result;
                    } else
                    {
                        result.player_state = state;
                        return result;
                    }
                }
            }
        }

        /// <summary>
        /// This should be called on all players when <see cref="AnswerPendingResult.player_state"/> == <see cref="State.AwaitingOtherPlayers"/> and <see cref="AnswerPendingResult.all_is_ready"/> == true.
        /// </summary>
        /// <returns></returns>
        public State InArena(IArena arena)
        {
            lock(mutex)
            {
                if (state != State.AwaitingOtherPlayers)
                    return state;
                state = State.InArena;
                this.arena = arena;
                return state;
            }
        }

        public struct CancelResult
        {
            public State new_state;
            internal MatchDecl match_decl;
        }

        /// <summary>
        /// Returns the current state.
        /// If the state == <see cref="State.InArena"/>
        /// then the arena is not cancelled.
        /// Call LeaveArena to cancel that.
        /// </summary>
        /// <returns></returns>
        public CancelResult Cancel()
        {
            lock(mutex)
            {
                var result = new CancelResult
                {
                    match_decl = match_player_decl
                };
                if (state == State.InArena)
                {
                    result.new_state = State.InArena;
                    return result;
                }
                state = State.NotQueued;
                result.new_state = State.NotQueued;
                return result;
            }
        }

        /// <summary>
        /// Returns the new state.
        /// If the state is <see cref="State.InArena"/> then state is set to <see cref="State.NotQueued"/>.
        /// In all other cases the state is unmodified and that state is returned instead.
        /// </summary>
        /// <returns></returns>
        public State LeaveArena(Player player)
        {
            lock(mutex)
            {
                if (state != State.InArena)
                    return state;
                arena.RemoveLeftPlayer(player);
                arena.RemoveRightPlayer(player);
                arena = null;
                state = State.NotQueued;
                return State.NotQueued;
            }
        }
    }
}
