using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private float startTime;
    private float currentTime;
    private bool done;
    public bool Done { get { return done; } }
    public Action eventCallback;

    public Timer(float time, Action eventCallback = null)
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
            eventCallback();
        }
    }

    public void Reset()
    {
        currentTime = startTime;
        done = false;
    }
}
