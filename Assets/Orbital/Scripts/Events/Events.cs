using UnityEngine.Events;
using System;
using UnityEngine;

[Serializable]
public class ScreenTouchedEvent : UnityEvent<Vector2>
{
}

[Serializable]
public class PlanetEvent : UnityEvent<Planet>
{
}

[Serializable]
public class IntEvent : UnityEvent<int>
{
}