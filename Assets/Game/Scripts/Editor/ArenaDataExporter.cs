using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.IO;
using Google.Protobuf;

public class ArenaDataExporter : MonoBehaviour
{

    [MenuItem("CH/Export Scene Data")]
    public static void ExportSceneData()
    {
        var path = @".\MainServer\bin\Debug\scene_data.bin";
        var arena_decl_name = "ArenaDecl";
        var arena_data = new Arenadata.ArenaData();

        {
            var scene = SceneManager.GetActiveScene();
            var root_objects = scene.GetRootGameObjects();
            
            foreach (var go in root_objects)
            {
                find_rigid_bodies(go, arena_data);
            }
        }

        {
            var arena_decl_go = GameObject.Find(arena_decl_name);
            if (arena_decl_go == null)
                throw new Exception(string.Format("Can not find a GameObject named '{0}'", arena_decl_name));
            var arena_decl = arena_decl_go.GetComponent<ArenaDecl>();
            if (arena_decl == null)
                throw new Exception(string.Format("The GameObject '{0}' is expected to contain a ArenaDecl component.", arena_decl_name));

            foreach(var spawnable in arena_decl.Spawnable)
            {
                var pos = Vector3.zero;
                var rot = spawnable.transform.rotation;
                var scale = spawnable.transform.localScale; //Does not have any parent
                var actor = create_actor(spawnable, pos, rot, scale);
                arena_data.SpawnableActors.Add(actor);
            }
        }

        using (var file_stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            arena_data.WriteTo(file_stream);
        Debug.Log("Success!");
    }

    static void find_rigid_bodies(GameObject go, Arenadata.ArenaData arena_data)
    {
        var pos = go.transform.position;
        var rotation = go.transform.rotation;
        var scale = go.transform.localScale;
        find_rigid_bodies_impl(go, pos, rotation, scale, arena_data);
    }

    static void find_rigid_bodies_impl(GameObject go, Vector3 pos, Quaternion rot, Vector3 scale, Arenadata.ArenaData arena_data)
    {
        var rb = go.GetComponent<Rigidbody>();
        if (rb != null)
        {
            var actor = create_actor(rb, pos, rot, scale);
            if (go.isStatic)
            {
                arena_data.StaticActors.Add(actor);
            }
            else
            {
                arena_data.DynamicActors.Add(actor);
            }
        }
        else
        {
            if (go.GetComponent<Collider>() != null)
                throw new Exception(string.Format("GameObject '{0}' have a collider but not any rigid body.", go.name));

            for (var i = 0; i < go.transform.childCount; ++i)
            {
                var child_t = go.transform.GetChild(i);
                var child_pos = child_t.localPosition;
                var child_rot = child_t.localRotation;
                var child_scale = calc_scale(scale, child_t);
                find_rigid_bodies_impl(child_t.gameObject, child_pos, child_rot, child_scale, arena_data);
            }
        }
    }

    private static Arenadata.Actor create_actor(Rigidbody rb, Vector3 pos, Quaternion rot, Vector3 scale)
    {
        var actor = new Arenadata.Actor
        {
            AffectedByGravity = rb.useGravity,
            Mass = rb.mass
        };
        actor.Transform = new Arenadata.Transform
        {
            Position = to_arena_type(pos),
            Velocity = to_arena_type(Vector3.zero),
            Rotation = to_arena_type(rot)
        };
        collect_child_collider_data(rb.gameObject, pos, rot, scale, actor.Colliders);
        return actor;
    }

    private static Vector3 calc_scale(Vector3 scale, Transform child_t)
    {
        return new Vector3
        {
            x = child_t.localScale.x * scale.x,
            y = child_t.localScale.y * scale.y,
            z = child_t.localScale.z * scale.z,
        };
    }

    static void collect_child_collider_data(GameObject go, Vector3 pos, Quaternion rot, Vector3 scale, Google.Protobuf.Collections.RepeatedField<Arenadata.Collider> sink)
    {
        var colliders = go.GetComponents<Collider>();
        if (colliders != null && colliders.Length > 0)
        {
            if(colliders.Length == 1) //There are only one collider. So there can be child colliders
            {
                var data = create_collider_data(pos, rot, scale, colliders[0]);
                sink.Add(data);
                collect_child_collider_data_iterate(go, scale, data.Children);
            } else //There are multiple colliders on one game object. So there can not be any child colliders
            {
                foreach (var collider in colliders)
                {
                    var data = create_collider_data(pos, rot, scale, collider);
                    sink.Add(data);
                }
                check_no_child_colliders(go);
            }
        } else //There are no colliders, just iterate down
        {
            collect_child_collider_data_iterate(go, scale, sink);
        }
    }

    private static void check_no_child_colliders(GameObject go)
    {
        for (var i = 0; i < go.transform.childCount; ++i)
        {
            var child_t = go.transform.GetChild(i);
            if (child_t.GetComponent<Collider>() != null)
                throw new Exception(string.Format("GameObject '{0}' have a collider even though a parent collider has multiple colliders.", child_t.gameObject.name));
            check_no_child_colliders(child_t.gameObject);
        }
    }

    private static void collect_child_collider_data_iterate(GameObject go, Vector3 scale, Google.Protobuf.Collections.RepeatedField<Arenadata.Collider> sink)
    {
        for (var i = 0; i < go.transform.childCount; ++i)
        {
            var child_t = go.transform.GetChild(i);
            var child_pos = child_t.localPosition;
            var child_rot = child_t.localRotation;
            var child_scale = calc_scale(scale, child_t);

            if (child_t.GetComponent<Rigidbody>() != null)
                throw new Exception("There are stacked rigid bodies in scene.");

            collect_child_collider_data(child_t.gameObject, child_pos, child_rot, child_scale, sink);
        }
    }

    private static Arenadata.Collider create_collider_data(Vector3 pos, Quaternion rot, Vector3 scale, Collider collider)
    {
        {
            var box_c = collider as BoxCollider;
            if (box_c != null)
            {
                var s = box_c.size;
                var box_data = new Arenadata.BoxShape();
                var local_pos = pos + VectorOps.Mul(box_c.center, scale);
                box_data.Center = to_arena_type(local_pos);
                box_data.Rotation = to_arena_type(rot);
                box_data.Length = s.z;
                box_data.Height = s.y;
                box_data.Width = s.x;
                var collider_data = new Arenadata.Collider();
                collider_data.Box = box_data;
                return collider_data;
            }
        }

        {
            var sphere_c = collider as SphereCollider;
            if (sphere_c != null)
            {
                var sphere_data = new Arenadata.SphereShape();
                var local_pos = pos + VectorOps.Mul(sphere_c.center, scale);
                sphere_data.Rotation = to_arena_type(rot);
                sphere_data.Center = to_arena_type(local_pos);
                sphere_data.Radius = sphere_c.radius;
                var collider_data = new Arenadata.Collider();
                collider_data.Sphere = sphere_data;
                return collider_data;
            }
        }

        {
            var capsule_c = collider as CapsuleCollider;
            if(capsule_c != null)
            {
                var capsule_data = new Arenadata.CapsuleShape();
                var local_pos = pos + VectorOps.Mul(capsule_c.center, scale);
                capsule_data.Rotation = to_arena_type(rot);
                capsule_data.Center = to_arena_type(local_pos);
                capsule_data.Radius = capsule_c.radius;
                var collider_data = new Arenadata.Collider();
                collider_data.Capsule = capsule_data;
                return collider_data;
            }
        }

        throw new Exception("Scene contains an unsupported collider.");
    }

    private static Arenadata.Vector3 to_arena_type(Vector3 v)
    {
        return new Arenadata.Vector3
        {
            X = v.x,
            Y = v.y,
            Z = v.z,
        };
    }

    private static Arenadata.Quaternion to_arena_type(Quaternion q)
    {
        return new Arenadata.Quaternion
        {
            X = q.x,
            Y = q.y,
            Z = q.z,
            W = q.w
        };
    }
}
