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
    public float Target;
    private float _initialZoom;
    private Camera _camera;

    void Start ()
    {
        this._camera = GetComponent<Camera> ();
        this._initialZoom = _camera.orthographicSize;
        this.Target = this._initialZoom;
    }
    
    void Update ()
    {
        _camera.orthographicSize = Mathf.Lerp (_camera.orthographicSize, Target, Time.deltaTime);
    }

    public void IncrementTarget (float amount)
    {
        this.Target += amount;
    }

    public void Reset ()
    {
        this.Target = this._initialZoom;
    }
}
