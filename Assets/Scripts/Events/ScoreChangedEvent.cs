using UnityEngine.Events;
using System;

[Serializable]
/// <summary>
/// Score changed event.  First parameter is old score, second is new score
/// </summary>
public class ScoreChangedEvent : UnityEvent<int, int>
{
}
