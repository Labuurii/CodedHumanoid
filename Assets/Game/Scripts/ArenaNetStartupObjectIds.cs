using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArenaNetStartupObjectIds : MonoBehaviour
{
    /// <summary>
    /// The indexes are the ids.
    /// The range Objects.Count -> int.Max (inclusive) are the range for custom spawned objects.
    /// </summary>
    public List<Rigidbody> Objects = new List<Rigidbody>();

    private void Awake()
    {
        init();
    }

    private void OnValidate()
    {
        if (Application.isEditor && !Application.isPlaying)
            init();
    }

    private void init()
    {
        var scene = SceneManager.GetActiveScene();
        var root_objects = scene.GetRootGameObjects();
        foreach (var root_object in root_objects)
        {
            find_rigid_bodies(root_object);
        }
    }

    private void find_rigid_bodies(GameObject go)
    {
        var rb = go.GetComponent<Rigidbody>();
        if (rb != null)
            Objects.Add(rb);
        else
        {
            for(var i = 0; i < go.transform.childCount; ++i)
            {
                find_rigid_bodies(go.transform.GetChild(i).gameObject);
            }
        }
    }
}
