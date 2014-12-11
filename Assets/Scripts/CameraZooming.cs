using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraZooming : MonoBehaviour
{
    [Tooltip("How long the zoom should take, in seconds")]
    public float
        ZoomDuration = .5F;
    [Tooltip("How much the camera should zoom out for each new planet")]
    public float
        ZoomIncrement = 1;
    private float _target;
    private float _initialZoom;

    void Start ()
    {
        this._initialZoom = this.camera.orthographicSize;
        this._target = this._initialZoom;
    }
    
    // Update is called once per frame
    void Update ()
    {
        this.camera.orthographicSize = Mathf.Lerp (this.camera.orthographicSize, this._target, Time.deltaTime);
    }

    public void IncrementTarget (float amount)
    {
        this._target += amount;
    }

    public void ResetZoom ()
    {
        this._target = this._initialZoom;
    }

}
