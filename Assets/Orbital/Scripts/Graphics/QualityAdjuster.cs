using UnityEngine;
using UnityEngine.Events;
using System;

[DisallowMultipleComponent]
public abstract class QualityAdjuster : MonoBehaviour
{
    public abstract void SetQuality(int index);
}

