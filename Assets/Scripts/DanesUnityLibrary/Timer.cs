using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private float startTime;
    private float currentTime;
    private bool done;
    public bool Done { get { return done; } }

    public Timer(float time)
    {
        startTime = time;
        currentTime = startTime;
    }

    public void DecrementTimer(float decrement)
    {
        currentTime -= decrement;

        if (currentTime <= 0)
        {
            done = true;
        }
    }

    public void Reset()
    {
        currentTime = startTime;
        done = false;
    }
}
