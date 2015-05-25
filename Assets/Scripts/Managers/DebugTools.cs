using UnityEngine;
using System.Collections;
using System.Diagnostics;

public static class DebugTools
{
    [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
    public static void Assert(bool condition) {
        if (!condition) {
            throw new System.ApplicationException();
        }
    }

    [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
    public static void Assert (bool condition, string message)
    {
        if (!condition) {
            throw new System.ApplicationException(message);
        }
    }
}
