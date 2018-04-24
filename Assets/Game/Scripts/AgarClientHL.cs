using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArenaServices;
using UnityEngine;

public class AgarClientHL : MonoBehaviour
{
    readonly Dictionary<int, Rigidbody> objects = new Dictionary<int, Rigidbody>();
    readonly AgarClientLL client = new AgarClientLL();

    public ArenaNetStartupObjectIds StartUpIds;
    public ArenaDecl ArenaDecl;

    private void Start()
    {
        for(var i = 0; i < StartUpIds.Objects.Count; ++i)
        {
            var obj = StartUpIds.Objects[i];
            objects.Add(i, obj);
        }
        client.OnTransformChanged += on_transform_changed;
        client.OnObjectNotification += on_object_notification;
        client.Connect();
    }

    private void on_object_notification(Event3D_ObjectNotification notification)
    {
        var id = notification.Id;
        var spawn_id = notification.SpawnId;
        var is_spawn = notification.Spawn;
        var transform = notification.Transform;

        if(transform.Position == null)
        {
            Debug.LogError("Got object notification but there are no position of the object");
            return;
        }
        var pos = Conv.ToUnityType(transform.Position);
        var vel = Conv.ToUnityTypeOrDefault(transform.Velocity, UnityEngine.Vector3.zero);
        var rot = Conv.ToUnityTypeOrDefault(transform.Rotation, UnityEngine.Quaternion.identity);

        if(transform.Rotation == null)
        {
            Debug.LogError("Got object notification but there are no rotation of the object");
            return;
        }

        if(spawn_id < 0 || spawn_id >= ArenaDecl.Spawnable.Length)
        {
            Debug.LogError("Got ObjectNotification which does not have any valid spawn id.");
            return;
        }

        if(is_spawn)
        {
            if(objects.ContainsKey(id))
            {
                Debug.LogErrorFormat("Got spawn event of object id '{0}' which already exists. Chooses to keep old object.", id);
                return;
            }
        }

        var prefab = ArenaDecl.Spawnable[spawn_id];
        var go = Instantiate(prefab.gameObject, pos, rot);
        var rb = go.GetComponent<Rigidbody>();
        rb.velocity = vel;
    }

    private void on_transform_changed(Event3D_ObjectTransformChanged transform)
    {
        Rigidbody rb;
        if (objects.TryGetValue(transform.Id, out rb))
        {
            var t = transform.NewTransform;
            rb.position = Conv.ToUnityType(t?.Position);
            rb.velocity = Conv.ToUnityType(t?.Velocity);
            rb.rotation = Conv.ToUnityType(t?.Rotation);
        } else
        {
            Debug.LogErrorFormat("Could not find object id '{0}'", transform.Id);
        }
    }
}
