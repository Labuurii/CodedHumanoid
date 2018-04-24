using Jitter;
using MainServer.Arenas;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MainServer
{
    internal class Physics3DScheduler
    {
        /// <summary>
        /// Only used by step_all and therefore does not need synchronization.
        /// That is what add_queue and remove_queue is for.
        /// </summary>
        readonly WorkList<WorldInfo> work = WorkList<WorldInfo>.Create();
        readonly Thread physics_thread;

        const double STEP = 1 / 200;
        const double NETWORK_STEP = 1 / 10;

        internal static readonly Physics3DScheduler Instance = new Physics3DScheduler();

        struct WorldInfo
        {
            internal World world;
            internal Arena3D arena;
        }

        private Physics3DScheduler()
        {
            physics_thread = new Thread(run_all_physics);
            physics_thread.Name = "Physics";
            physics_thread.IsBackground = true; //TODO: Maybe there is a nicer way. This might do more than just cancel on main exit
            physics_thread.Start();
        }

        /// <summary>
        /// Threadsafe.
        /// </summary>
        /// <param name="world"></param>
        public void AddWorld(World world, Arena3D arena)
        {
            work.Add(new WorldInfo
            {
                world = world,
                arena = arena
            });
        }

        /// <summary>
        /// Threadsafe.
        /// </summary>
        /// <param name="world"></param>
        /// <param name="cb"></param>
        public void RemoveWorld(World world, Arena3D arena)
        {
            work.Remove(new WorldInfo
            {
                world = world,
                arena = arena
            });
        }

        private void run_all_physics()
        {
            CyclicTimer physics_timer = new CyclicTimer(STEP);
            CyclicTimer network_timer = new CyclicTimer(NETWORK_STEP);

            for (;;)
            {
                physics_timer.WaitUntilNextFrame();
                step_all((float)STEP, network_timer.HasPassedLastFrame());
                if(physics_timer.IsBehind())
                {
                    //TODO: Log once
                }
            }
        }

        private void step_all(float delta_time, bool sync_with_players)
        {
            work.Update();
            foreach(var world in work.List)
            {
                world.world.Step(delta_time, false);
                world.arena.Run(delta_time);
                if(sync_with_players)
                    world.arena.SyncPlayersAsync();
            }
        }
    }
}
