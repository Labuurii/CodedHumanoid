using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MainThreadEventQueue : MonoBehaviour
{
    static readonly ConcurrentQueue<Action> events = new ConcurrentQueue<Action>();

    private void LateUpdate()
    {
        Action a;
        while (events.TryDequeue(out a))
        {
            a();
        }
    }

    public static void Enqueue(Action a)
    {
        if (a == null)
            throw new ArgumentNullException("a");
        events.Enqueue(a);
    }
}
