using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class WaterDecl : MonoBehaviour
{
    public static WaterDecl Instance;

    private void Awake()
    {
        Debug.Assert(Instance == null, "There are multiple water instances.");
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
