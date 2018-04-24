using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public static class MainThread<T>
    where T : class, new()
{
    static Thread main_thread = Thread.CurrentThread;
    static T instance;

    public static T Get()
    {
        Debug.Assert(main_thread == Thread.CurrentThread, "MainThread<T>.Get() is not called on the main thread");
        if (instance == null)
            instance = new T();
        return instance;
    }
}
