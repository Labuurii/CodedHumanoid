using ArenaServices;
using Grpc.Core;
using MainServer.Matchmaking;
using ArenaHost;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainServer.Services
{
    internal class MatchMakerServiceImpl : MatchMakerService.MatchMakerServiceBase
    {
        internal MatchEngineLookUp match_constructors;

        public MatchMakerServiceImpl(MatchEngineLookUp all_match_finders)
        {
            this.match_constructors = all_match_finders;
        }

        public override Task<QueueStateMsg> AnswerMatch(AnswerMatchAttempt request, ServerCallContext context)
        {
            var player = PlayerAuth.Instance.GetPlayer(context);
            if (player == null)
                return not_authenticated();

            var result = player.match_maker.AnswerPending(player, request.Accepted);
            switch (result.player_state)
            {
                case MatchMaker.State.AwaitingOtherPlayers: //Success
                    {
                        if(result.all_is_ready)
                        {
                            //Create arena
                            var arena = match_constructors.GetArenaFactory(result.match_mode, result.arena, result.team_size).Construct();
                            foreach (var match_player in result.match_decl.Left)
                                arena.AddLeftPlayer(match_player);
                            foreach (var match_player in result.match_decl.Right)
                                arena.AddRightPlayer(match_player);

                            //Send arena joinable to everyone
                            var ev = new QueueStateMsg
                            {
                                Authenticated = true,
                                CurrentState = QueueState.ArenaJoinable
                            };

                            var players = result.match_decl.Left.Concat(result.match_decl.Right);
                            foreach(var match_player in players)
                            {
                                if(match_player != player)
                                {
                                    match_player.stream_match_maker.Enqueue(ev);
                                }
                                match_player.match_maker.InArena(arena);
                            }

                            //And finally send to last player
                            return Task.FromResult(ev);
                        } else
                        {
                            return Task.FromResult(new QueueStateMsg
                            {
                                Authenticated = true,
                                CurrentState = QueueState.AwaitingOtherPlayers
                            });
                        }
                    }
                case MatchMaker.State.AwaitingAccept:
                case MatchMaker.State.InArena:
                case MatchMaker.State.NotQueued:
                case MatchMaker.State.Queued:
                    return state_task(result.player_state);
                default:
                    throw new Exception("Unhandled enum value " + result.player_state);
            }
        }

        public override Task<QueueStateMsg> LeaveQueue(LeaveQueueAttempt request, ServerCallContext context)
        {
            var player = PlayerAuth.Instance.GetPlayer(context);
            if (player == null)
                return not_authenticated();

            var result = player.match_maker.Cancel();
            switch(result.new_state)
            {
                case MatchMaker.State.NotQueued:
                    var ev = new_state(MatchMaker.State.NotQueued);
                    var players = result.match_decl.Left.Concat(result.match_decl.Right); //TODO: Does Concat handle the null possibility?
                    foreach(var match_player in players)
                    {
                        match_player.stream_match_maker.Enqueue(ev);
                    }
                    return Task.FromResult(ev);

                case MatchMaker.State.AwaitingAccept:
                case MatchMaker.State.AwaitingOtherPlayers:
                case MatchMaker.State.Queued:
                case MatchMaker.State.InArena:
                    return state_task(result.new_state);
                default:
                    throw new Exception("Unhandle enum value " + result.new_state);
            }
        }

        public override Task<QueueStateMsg> Queue(QueueAttempt request, ServerCallContext context)
        {
            var player = PlayerAuth.Instance.GetPlayer(context);
            if (player == null)
                return not_authenticated();

            var match_finder = match_constructors.GetMatchFinder(request.MatchMode, request.Arena, request.TeamSize);
            if(match_finder == null)
            {
                return state_task(MatchMaker.State.NotQueued);
            }

            match_finder.AddPlayer(player);
            return state_task(player.match_maker.Queue(request.MatchMode, request.Arena, request.TeamSize));
        }


        private static Task<QueueStateMsg> state_task(MatchMaker.State new_state)
        {
            return Task.FromResult(MatchMakerServiceImpl.new_state(new_state));
        }

        private static QueueStateMsg new_state(MatchMaker.State new_state)
        {
            QueueState t;
            switch (new_state)
            {
                case MatchMaker.State.Queued:
                    t = QueueState.Queued;
                    break;
                case MatchMaker.State.NotQueued:
                    t = QueueState.NotQueued;
                    break;
                case MatchMaker.State.InArena:
                    t = QueueState.InArena;
                    break;
                case MatchMaker.State.AwaitingOtherPlayers:
                    t = QueueState.AwaitingOtherPlayers;
                    break;
                case MatchMaker.State.AwaitingAccept:
                    t = QueueState.QueuePopped;
                    break;
                default:
                    throw new Exception("Unhandled enum value " + new_state);
            }

            return new QueueStateMsg
            {
                Authenticated = true,
                CurrentState = t
            };
        }

        private static Task<QueueStateMsg> not_authenticated()
        {
            return Task.FromResult(new QueueStateMsg
            {
                Authenticated = false
            });
        }

        public override async Task Subscribe(SubscriptionAttempt request, IServerStreamWriter<QueueStateMsg> responseStream, ServerCallContext context)
        {
            var player = PlayerAuth.Instance.GetPlayer(context);
            if (player == null || player.stream_match_maker.SetStream(responseStream, context))
                return;

            try
            {
                for(; ; )
                {
                    if (!player.online || !await player.stream_match_maker.SendCurrentEvents())
                        break;
                    await Task.Delay(5);
                }
            }
            catch (InvalidOperationException)
            {
                //Happens when cancellation token is set to true
            }
            catch (RpcException)
            {
                //Nothing special
            }
            catch (Exception e)
            {
                Log.Exception(e);
            } finally
            {
                player.match_maker.Cancel();
            }
        }
    }
}
