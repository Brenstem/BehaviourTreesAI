using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private float startTime;
    private float currentTime;
    public bool Done { get { return currentTime <= 0; } }
    public System.Action eventCallback;

    public Timer(float time, System.Action eventCallback = null)
    {
        startTime = time;
        currentTime = startTime;
    }

    public void DecrementTimer(float decrement)
    {
        currentTime -= decrement;

        if (currentTime <= 0)
        {
            if(eventCallback != null)
                eventCallback();
        }
    }

    public void Reset()
    {
        currentTime = startTime;
    }
    
    public void Reset(float newTime)
    {
        startTime = newTime;
        currentTime = startTime;
    }
}
