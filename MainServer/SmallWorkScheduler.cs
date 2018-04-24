using MainServer.Matchmaking;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MainServer
{
    internal class SmallWorkScheduler
    {
        const double STEP = 5;
        const double QUEUE_POP_TIMEOUT = 30;

        readonly ConcurrentQueue<MatchDeclWork> match_decls_in_flux = new ConcurrentQueue<MatchDeclWork>();
        List<IMatchFinder> match_finders; //Requires init in main and is not reassignable
        readonly Thread worker;

        struct MatchDeclWork
        {
            internal MatchDecl match_decl;
            internal DateTime added;
        }

        internal static readonly SmallWorkScheduler Instance = new SmallWorkScheduler();

        private SmallWorkScheduler()
        {
            worker = new Thread(run_small_work_forever);
            worker.IsBackground = true;
            worker.Name = "Small Work";
            worker.Start();
        }

        internal void SetMatchFinders(List<IMatchFinder> finders)
        {
            this.match_finders = finders;
        }

        internal void AddMatchDecl(MatchDecl decl)
        {
            match_decls_in_flux.Enqueue(new MatchDeclWork
            {
                match_decl = decl,
                added = DateTime.Now
            });
        }

        private void run_small_work_forever()
        {
            List<MatchDeclWork> reused_requeued_match_decls = new List<MatchDeclWork>();
            CyclicTimer timer = new CyclicTimer(STEP);

            for(; ; )
            {
                if(timer.IsBehind())
                {
                    //TODO: Log once every 10 minutes
                }
                timer.WaitUntilNextFrame();

                //Remove to old queues
                {
                    MatchDeclWork work;
                    while (match_decls_in_flux.TryDequeue(out work))
                    {
                        if (!work.match_decl.AllHasAccepted())
                        {
                            var delta = DateTime.Now - work.added;
                            if (delta.TotalSeconds > QUEUE_POP_TIMEOUT)
                            {
                                work.match_decl.TimeOut();
                            }
                            else
                            {
                                reused_requeued_match_decls.Add(work);
                            }
                        }
                    }

                    foreach (var requeued_work in reused_requeued_match_decls)
                    {
                        match_decls_in_flux.Enqueue(requeued_work);
                    }
                    reused_requeued_match_decls.Clear();
                }


                //Run the match finders
                if(match_finders != null) {
                    foreach (var f in match_finders)
                        f.Run();
                }
            }
        }
    }
}
