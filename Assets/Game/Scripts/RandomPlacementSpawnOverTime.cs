using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RandomPlacementSpawnOverTime : NetworkBehaviour {
    CyclicTimer spawn_timer;
    Vector2 bottom, top;

    public SpawnMaster Master;
    public float SpawnPulseDelta;
    public float SpawnCount;
    public Vector2 Size;
    public bool SpawnAtGround;
    public float GroundOffset;
    public bool SpawnAtStartUp;

    private void Start()
    {
        foreach (var c in GetComponents<Collider>())
            Destroy(c);

        if(SpawnAtStartUp)
        {
            spawn_weapons();
        }
    }

    private void OnValidate()
    {
        spawn_timer = new CyclicTimer(SpawnPulseDelta);

        var half_size = Size / 2;
        var p = transform.position;
        bottom = new Vector2
        {
            x = p.x,
            y = p.z
        } - half_size;
        top = new Vector2
        {
            x = p.x,
            y = p.z
        } + half_size;
    }

    private void Update()
    {
        if (!isServer || !Master.ShouldSpawnNewWeapon())
            return;

        var has_passed = spawn_timer.HasPassedLastFrame();
        if(has_passed)
        {
            spawn_weapons();
        }
    }

    private void spawn_weapons()
    {
        for (var si = 0; si < SpawnCount; ++si)
        {
            var pos = new Vector3
            {
                x = Random.Range(bottom.x, top.x),
                z = Random.Range(bottom.y, top.y),
                y = transform.position.y
            };

            if (SpawnAtGround)
            {
                RaycastHit hit;
                if (Physics.Raycast(pos, Vector3.down, out hit))
                {
                    pos.y = hit.point.y + GroundOffset;
                }
            }

            Master.SpawnRandomWeapon(pos);
        }
    }
}
