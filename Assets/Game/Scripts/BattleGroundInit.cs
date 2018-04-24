using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleGroundInit : MonoBehaviour
{
    void Start()
    {
        Physics.IgnoreLayerCollision((int)Layer.InvisibleWall, (int)Layer.Bullet, true);
    }
}