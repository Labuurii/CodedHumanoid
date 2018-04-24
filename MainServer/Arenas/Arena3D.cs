using ArenaServices;
using Jitter;
using Jitter.Collision;
using Jitter.Dynamics;
using Jitter.LinearMath;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainServer.Arenas
{
    internal abstract class Arena3D : ArenaBase
    {
        protected readonly CollisionSystem cs;
        protected readonly World world;
        
        private readonly SortedList<int, ObjectData> objects;
        private readonly List<RigidBody> spawnable_rigid_bodies;
        private int startobject_high_edge;
        private Random random;

        struct ObjectData
        {
            internal RigidBody rb;
            /// <summary>
            /// Networking data.
            /// </summary>
            internal JVector last_pos, last_vel, last_ang_vel;

            /// <summary>
            /// Networking data.
            /// </summary>
            internal JMatrix last_ori;

            /// <summary>
            /// Only set if it was spawned by script.
            /// </summary>
            internal int spawn_id;
        }

        /// <summary>
        /// Does not schedule the arena for physics updates. That has to happen in subclass constructor.
        /// Just make sure to remove the world from the physics engine later.
        /// </summary>
        /// <param name="cs"></param>
        internal Arena3D(CollisionSystem cs)
        {
            this.cs = cs;
            world = new World(cs);
            objects = new SortedList<int, ObjectData>();
            spawnable_rigid_bodies = new List<RigidBody>();
            random = new Random();

            Physics3DScheduler.Instance.AddWorld(world, this);
        }

        /// <summary>
        /// Can only be called once and has to be called before Run.
        /// Part of initialization.
        /// </summary>
        /// <param name="bodies"></param>
        public void SetStartupRigidbodies(IEnumerable<RigidBody> bodies)
        {
            int id = 0;
            foreach(var body in bodies)
            {
                add_to_world(id, -1, body);
                ++id;
            }
            startobject_high_edge = id;
        }

        private void add_to_world(int id, int spawn_id, RigidBody body)
        {
            objects.Add(id, new ObjectData
            {
                rb = body,
                spawn_id = spawn_id
            });
            //body.Tag = new RigidBodyTag(id); Currently not necessary
            world.AddBody(body);
        }

        /// <summary>
        /// Can only be called once and has to be called ebfore Run.
        /// Part of initialization.
        /// </summary>
        /// <param name="bodies"></param>
        public void SetSpawnableRigidbodies(IEnumerable<RigidBody> bodies)
        {
            Debug.Assert(spawnable_rigid_bodies.Count == 0, "SetSpawnableRigidbodies can only be called at startup");
            spawnable_rigid_bodies.AddRange(bodies);
        }

        public override void AddLeftPlayer(Player player)
        {
            base.AddLeftPlayer(player);
            send_spawned_objects_to_player(player);
        }

        public override void AddRightPlayer(Player player)
        {
            base.AddRightPlayer(player);
            send_spawned_objects_to_player(player);
        }

        private void send_spawned_objects_to_player(Player player)
        {
            foreach(var kv in objects)
            {
                if(kv.Key >= startobject_high_edge) //Is a spawned object
                {
                    var ev = new Event3D
                    {
                        ObjectNotification = new Event3D_ObjectNotification
                        {
                            Id = kv.Key,
                            SpawnId = kv.Value.spawn_id,
                            Spawn = false,
                            Transform = ArenaServiceConv.ToArenaTransform(kv.Value.rb)
                        }
                    };

                    send_to_all_players(ev);
                }
            }
        }

        public void SyncPlayersAsync()
        {
            foreach(var kv in objects)
            {
                var id = kv.Key;
                var data = kv.Value;
                var body = data.rb;
                var pos = body.Position;
                var vel = body.LinearVelocity;
                var ori = body.Orientation;
                var ang_vel = body.AngularVelocity;

                var pos_distance = VectorOps.Distance(data.last_pos, pos);
                var vel_distance = VectorOps.Distance(data.last_vel, vel);
                var ang_distance = VectorOps.Distance(data.last_ang_vel, vel);
                if (pos_distance > 0.1f
                    || vel_distance > 0.1f
                    || MatrixOps.CmpExact(data.last_ori, ori) //TODO: Do some approximation
                    || ang_distance > 0.1f) 
                {
                    data.last_pos = body.Position;
                    data.last_vel = body.LinearVelocity;
                    data.last_ori = body.Orientation;
                    data.last_ang_vel = body.AngularVelocity;

                    var ev = new Event3D
                    {
                        ObjectTransformChanged = new Event3D_ObjectTransformChanged
                        {
                            Id = id,
                            NewTransform = new Transform
                            {
                                Position = ArenaServiceConv.ToArenaData(pos),
                                Velocity = ArenaServiceConv.ToArenaData(vel),
                                Rotation = ArenaServiceConv.ToArenaData(ori),
                                AngularVelocity = ArenaServiceConv.ToArenaData(ang_vel)
                            }
                        }
                    };

                    send_to_all_players(ev);
                }
            }
        }

        /// <summary>
        /// Single threaded.
        /// </summary>
        /// <param name="spawnable_idx"></param>
        /// <returns></returns>
        protected KeyValuePair<int, RigidBody> SpawnObject(int spawnable_idx, JVector? pos = null, JVector? vel = null, JQuaternion? ori = null)
        {
            var rb = new RigidBody(spawnable_rigid_bodies[spawnable_idx].Shape);
            if (pos.HasValue)
                rb.Position = pos.Value;
            if (vel.HasValue)
                rb.LinearVelocity = vel.Value;
            if (ori.HasValue)
                rb.Orientation = JMatrix.CreateFromQuaternion(ori.Value);
            for(;;)
            {
                var id = random.Next(startobject_high_edge, int.MaxValue);
                if(!objects.ContainsKey(id))
                {
                    add_to_world(id, spawnable_idx, rb);
                    var ev = new Event3D
                    {
                        ObjectNotification = new Event3D_ObjectNotification
                        {
                            Id = id,
                            SpawnId = spawnable_idx,
                            Spawn = true,
                            Transform = ArenaServiceConv.ToArenaTransform(rb)
                        }
                    };
                    send_to_all_players(ev);
                    return new KeyValuePair<int, RigidBody>(id, rb);
                }
            }
        }

        protected RigidBody GetObject(int id)
        {
            ObjectData data;
            Debug.Assert(objects.TryGetValue(id, out data));
            return data.rb;
        }

        private void send_to_all_players(Event3D ev)
        {
            send_to_all_players_in_list(left_players, ev);
            send_to_all_players_in_list(right_players, ev);
        }

        private void send_to_all_players_in_list(AtomicData<List<Player>> players, Event3D ev)
        {
            players.Access((list) =>
            {
                foreach (var player in list)
                {
                    player.stream_3d.Enqueue(ev);
                }
            });
            
        }

        internal abstract void Run(float delta_time);

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Physics3DScheduler.Instance.RemoveWorld(world, this);
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public override void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
