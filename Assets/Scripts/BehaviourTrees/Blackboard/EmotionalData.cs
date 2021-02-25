using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionalData
{
    public EmotionalData(float happiness, float anxiety, float anger, float sadness, float exhaustion)
    {
        Happiness = happiness;
        Anxiety = anxiety;
        Anger = anger;
        Sadness = sadness;
        Exhaustion = exhaustion;
    }


    public float Happiness { get { return Mathf.Clamp(Happiness, 0, 1); } set { Happiness = value; } }
    public float Anxiety { get { return Mathf.Clamp(Anxiety, 0, 1); } set { Anxiety = value; } }
    public float Anger { get { return Mathf.Clamp(Anger, 0, 1); } set { Anger = value; } }
    public float Sadness { get { return Mathf.Clamp(Sadness, 0, 1); } set { Sadness = value; } }
    public float Exhaustion { get { return Mathf.Clamp(Exhaustion, 0, 1); } set { Exhaustion = value; } }
}
