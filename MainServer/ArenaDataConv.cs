using Arenadata;
using Google.Protobuf.Collections;
using Jitter.Collision.Shapes;
using Jitter.Dynamics;
using Jitter.LinearMath;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Jitter.Collision.Shapes.CompoundShape;

namespace MainServer
{
    internal static class ArenaDataConv
    {
        internal struct ConvResult
        {
            internal List<RigidBody> static_, dynamic, spawnable;
        }

        internal static Vector3 ToArenaData(JVector v)
        {
            return new Vector3
            {
                X = v.X,
                Y = v.Y,
                Z = v.Z,
            };
        }

        internal static Quaternion ToArenaData(JQuaternion v)
        {
            return new Quaternion
            {
                X = v.X,
                Y = v.Y,
                Z = v.Z,
                W = v.W
            };
        }

        internal static Quaternion ToArenaData(JMatrix v)
        {
            var q = JQuaternion.CreateFromMatrix(v);
            return ToArenaData(q);
        }

        internal static ConvResult ToCollisionData(ArenaData data)
        {
            var static_buffer = new List<RigidBody>();
            create_rigid_bodies(data.StaticActors, static_buffer);
            foreach (var @static in static_buffer)
                @static.IsStatic = true;

            var dynamic_buffer = new List<RigidBody>();
            create_rigid_bodies(data.DynamicActors, dynamic_buffer);

            var spawnable_buffer = new List<RigidBody>();
            create_rigid_bodies(data.SpawnableActors, spawnable_buffer);

            return new ConvResult
            {
                static_ = static_buffer,
                dynamic = dynamic_buffer,
                spawnable = spawnable_buffer
            };
        }

        private static void create_rigid_bodies(RepeatedField<Actor> actors, List<RigidBody> sink)
        {
            if (actors == null)
                return;

            foreach(var actor in actors)
            {
                if (actor.Transform == null)
                    throw new Exception("Actor has no transform");
                var t = actor.Transform;
                if (t.Position == null)
                    throw new Exception("Actor has no position");
                if (t.Velocity == null)
                    throw new Exception("Actor has no velocity");
                if (t.Rotation == null)
                    throw new Exception("Actor has no rotation");

                var compund = create_shapes(actor.Colliders);
                var rb = new RigidBody(compund);
                rb.Mass = actor.Mass;
                rb.AffectedByGravity = actor.AffectedByGravity;
                rb.Position = to_j(actor.Transform.Position);
                rb.LinearVelocity = to_j(actor.Transform.Velocity);
                rb.Orientation = to_j(actor.Transform.Rotation);

                sink.Add(rb);
            }
        }

        private static CompoundShape create_shapes(RepeatedField<Collider> colliders)
        {
            if (colliders == null || colliders.Count == 0)
                throw new Exception("Colliders is null or count is zero");

            var shape_buffer = new List<TransformedShape>();

            foreach(var collider in colliders)
            {
                Shape shape = null;
                Vector3 center;
                Quaternion rotation;

                switch(collider.ShapeCase)
                {
                    case Collider.ShapeOneofCase.None:
                        throw new Exception("Collider without any shape");
                    case Collider.ShapeOneofCase.Box:
                        var box = collider.Box;
                        shape = new Jitter.Collision.Shapes.BoxShape(box.Length, box.Height, box.Width);
                        center = box.Center;
                        rotation = box.Rotation;
                        break;
                    case Collider.ShapeOneofCase.Sphere:
                        var sphere = collider.Sphere;
                        shape = new Jitter.Collision.Shapes.SphereShape(sphere.Radius);
                        center = sphere.Center;
                        rotation = sphere.Rotation;
                        break;
                    case Collider.ShapeOneofCase.Capsule:
                        var capsule = collider.Capsule;
                        shape = new Jitter.Collision.Shapes.CapsuleShape(capsule.Height, capsule.Radius);
                        center = capsule.Center;
                        rotation = capsule.Rotation;
                        break;
                    default:
                        Debug.Assert(false, "Unhandled enum value " + collider.ShapeCase);
                        throw new Exception();
                }

                var center_j = to_j(center);
                var rotation_j = to_j(rotation);
                var transformed_shape = new TransformedShape(shape, JMatrix.Identity, center_j);
                shape_buffer.Add(transformed_shape);

                if(collider.Children != null && collider.Children.Count > 0)
                    create_shapes(collider.Children);
            }

            return new CompoundShape(shape_buffer);
        }

        private static JMatrix to_j(Quaternion r)
        {
            var q = new JQuaternion(r.X, r.Y, r.Z, r.W);
            return JMatrix.CreateFromQuaternion(q);
        }

        private static JVector to_j(Vector3 center)
        {
            return new JVector(center.X, center.Y, center.Z);
        }

        private static JVector add(JVector origin, Vector3 offset)
        {
            origin.X += offset.X;
            origin.Y += offset.Y;
            origin.Z += offset.Z;
            return origin;
        }
    }
}
