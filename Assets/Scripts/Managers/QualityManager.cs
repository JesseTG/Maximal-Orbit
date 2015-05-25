using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

[DisallowMultipleComponent]
public class QualityManager : MonoBehaviour
{
    public int Quality {
        get {
            return QualitySettings.GetQualityLevel ();
        }
        set {
            if (0 <= value && value < QualitySettings.names.Length) {
                QualitySettings.SetQualityLevel (value, true);
                foreach (QualityAdjuster i in GameObject.FindObjectsOfType<QualityAdjuster>()) {
                    i.SetQuality (value);
                }
            }
#if DEVELOPMENT_BUILD || UNITY_EDITOR
            else {
                Debug.LogErrorFormat (
                    this,
                    "Attempted to set quality to invalid value {0}",
                    value
                    );
            }
#endif
        }
    }

    private string[] _names;

    void Start ()
    {
        this._names = QualitySettings.names;
    }

    public void CycleQuality ()
    {
        this.Quality = (this.Quality + 1) % this._names.Length;
#if DEVELOPMENT_BUILD || UNITY_EDITOR
        Debug.LogFormat (
            this,
            "Cycled quality to {0} (Level {1}/{2})",
            this._names [this.Quality],
            this.Quality + 1,
            this._names.Length
        );
#endif
    }
}
