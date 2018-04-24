using System;
using System.Collections.Generic;
using MainServer;
using MainServer.Arenas;
using MainServer.Matchmaking;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MainServerTest
{
    [TestClass]
    public class MatchMakerTest
    {
        class DummyArena : ArenaBase
        {
            public override void Dispose()
            {
            }
        }

        [TestMethod]
        public void FullLineAccept()
        {
            var p1 = new Player();
            var p2 = new Player();
            var mm = MatchMaker.Create();
            Assert.AreEqual(MatchMaker.State.Queued, mm.Queue(ArenaServices.MatchMode.Skirmish, ArenaServices.Arena.Agar, ArenaServices.TeamSize.One));
            var match_decl = new MatchDecl(array_up(p1), array_up(p2));
            Assert.AreEqual(MatchMaker.State.AwaitingAccept, mm.AwaitingAccept(match_decl));
            Assert.AreEqual(new MatchMaker.AnswerPendingResult
            {
                all_is_ready = false,
                match_decl = match_decl,
                player_state = MatchMaker.State.AwaitingOtherPlayers
            }, mm.AnswerPending(p1, true));

            var mm2 = MatchMaker.Create();
            Assert.AreEqual(MatchMaker.State.Queued, mm2.Queue(ArenaServices.MatchMode.Skirmish, ArenaServices.Arena.Agar, ArenaServices.TeamSize.One));
            Assert.AreEqual(MatchMaker.State.AwaitingAccept, mm2.AwaitingAccept(match_decl));
            Assert.AreEqual(new MatchMaker.AnswerPendingResult
            {
                all_is_ready = true,
                match_decl = match_decl,
                player_state = MatchMaker.State.AwaitingOtherPlayers
            }, mm2.AnswerPending(p2, true));

            Assert.AreEqual(MatchMaker.State.InArena, mm.InArena(new DummyArena()));
        }

        [TestMethod]
        public void FullLineFirstRefused()
        {
            var p1 = new Player();
            var p2 = new Player();
            var mm = MatchMaker.Create();
            Assert.AreEqual(MatchMaker.State.Queued, mm.Queue(ArenaServices.MatchMode.Skirmish, ArenaServices.Arena.Agar, ArenaServices.TeamSize.One));
            var match_decl = new MatchDecl(array_up(p1), array_up(p2));
            Assert.AreEqual(MatchMaker.State.AwaitingAccept, mm.AwaitingAccept(match_decl));
            Assert.AreEqual(new MatchMaker.AnswerPendingResult
            {
                all_is_ready = false,
                match_decl = match_decl,
                player_state = MatchMaker.State.NotQueued
            }, mm.AnswerPending(p1, false));

            var mm2 = MatchMaker.Create();
            Assert.AreEqual(MatchMaker.State.Queued, mm2.Queue(ArenaServices.MatchMode.Skirmish, ArenaServices.Arena.Agar, ArenaServices.TeamSize.One));
            Assert.AreEqual(MatchMaker.State.AwaitingAccept, mm2.AwaitingAccept(match_decl));
            Assert.AreEqual(new MatchMaker.AnswerPendingResult
            {
                all_is_ready = false,
                match_decl = match_decl,
                player_state = MatchMaker.State.AwaitingOtherPlayers
            }, mm2.AnswerPending(p2, true));
        }

        [TestMethod]
        public void Cancel()
        {
            var p = new Player();
            var mm = MatchMaker.Create();
            var decl = new MatchDecl(array_up(p), new List<Player>());

            Assert.AreEqual(MatchMaker.State.NotQueued, mm.Cancel().new_state);

            mm.Queue(ArenaServices.MatchMode.Skirmish, ArenaServices.Arena.Agar, ArenaServices.TeamSize.One);
            Assert.AreEqual(MatchMaker.State.NotQueued, mm.Cancel().new_state);

            mm.Queue(ArenaServices.MatchMode.Skirmish, ArenaServices.Arena.Agar, ArenaServices.TeamSize.One);
            mm.AwaitingAccept(decl);
            Assert.AreEqual(MatchMaker.State.NotQueued, mm.Cancel().new_state);

            mm.Queue(ArenaServices.MatchMode.Skirmish, ArenaServices.Arena.Agar, ArenaServices.TeamSize.One);
            mm.AwaitingAccept(decl);
            mm.AnswerPending(p, true);
            Assert.AreEqual(MatchMaker.State.NotQueued, mm.Cancel().new_state);

            mm.Queue(ArenaServices.MatchMode.Skirmish, ArenaServices.Arena.Agar, ArenaServices.TeamSize.One);
            mm.AwaitingAccept(decl);
            mm.AnswerPending(p, true);
            mm.InArena(new DummyArena());
            Assert.AreEqual(MatchMaker.State.InArena, mm.Cancel().new_state);
        }

        [TestMethod]
        public void LeaveArena()
        {
            var p = new Player();
            var mm = MatchMaker.Create();
            var decl = new MatchDecl(array_up(p), new List<Player>());

            mm.Queue(ArenaServices.MatchMode.Skirmish, ArenaServices.Arena.Agar, ArenaServices.TeamSize.One);
            mm.AwaitingAccept(decl);
            mm.AnswerPending(p, true);
            mm.InArena(new DummyArena());
            Assert.AreEqual(MatchMaker.State.NotQueued, mm.LeaveArena());

            mm.Queue(ArenaServices.MatchMode.Skirmish, ArenaServices.Arena.Agar, ArenaServices.TeamSize.One);
            mm.AwaitingAccept(decl);
            mm.AnswerPending(p, true);
            Assert.AreEqual(MatchMaker.State.AwaitingOtherPlayers, mm.LeaveArena());

            mm.Cancel();
            mm.Queue(ArenaServices.MatchMode.Skirmish, ArenaServices.Arena.Agar, ArenaServices.TeamSize.One);
            mm.AwaitingAccept(decl);
            Assert.AreEqual(MatchMaker.State.AwaitingAccept, mm.LeaveArena());

            mm.Cancel();
            mm.Queue(ArenaServices.MatchMode.Skirmish, ArenaServices.Arena.Agar, ArenaServices.TeamSize.One);
            Assert.AreEqual(MatchMaker.State.Queued, mm.LeaveArena());

            mm.Cancel();
            Assert.AreEqual(MatchMaker.State.NotQueued, mm.LeaveArena());
        }

            private List<Player> array_up(params Player[] players)
        {
            return new List<Player>(players);
        }
    }
}
