using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public struct CyclicTimer
{
    double step;
    DateTime last_frame_end;

    /// <summary>
    /// Initializing with the default constructor is undefined behaviour.
    /// </summary>
    /// <param name="step_sec"></param>
    public CyclicTimer(double step_sec)
    {
        step = step_sec;
        last_frame_end = DateTime.Now + TimeSpan.FromSeconds(step_sec);
    }

    /// <summary>
    /// Always increases the frame.
    /// </summary>
    public void WaitUntilNextFrame()
    {
        var seconds_left = seconds_left_of_frame();
        if (seconds_left > 0)
        {
            Thread.Sleep((int)(seconds_left * 1000));
        }
        inc_frame();
    }

    /// <summary>
    /// If the frame has passed (function returns true) it increases the frame.
    /// </summary>
    /// <returns></returns>
    public bool HasPassedLastFrame()
    {
        var result = seconds_left_of_frame() <= 0;
        if (result)
        {
            inc_frame();
        }
        return result;
    }

    public bool IsBehind()
    {
        var next_frame = last_frame_end + TimeSpan.FromSeconds(step);
        var now = DateTime.Now;
        return now > next_frame;
    }

    private double seconds_left_of_frame()
    {
        var now = DateTime.Now;
        var delta = last_frame_end - now;
        return delta.TotalSeconds;
    }

    private void inc_frame()
    {
        last_frame_end += TimeSpan.FromSeconds(step);
    }
}
