using UnityEngine.Events;
using UnityEngine;
using System;

[Serializable]
public class Vector2Event : UnityEvent<Vector2>
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

[Serializable]
public class LeaderboardEvent : UnityEvent<string, long>
{

}

[Serializable]
public class StringEvent : UnityEvent<string>
{

}