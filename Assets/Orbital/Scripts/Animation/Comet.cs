using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(TrailRenderer), typeof(CircleCollider2D))]
public class Comet : MonoBehaviour
{
    public Vector2 InitialVelocity;
    public float MinRestTime;
    public float MaxRestTime;
    [Range(0, 1)]
    public float
        MinEntryRatio;
    [Range(0, 1)]
    public float
        MaxEntryRatio;
    private Rigidbody2D _body;
    private TrailRenderer _trail;
    private CircleCollider2D _collider;
    private Light _light;

    // Use this for initialization
    void Start ()
    {
        this._body = GetComponent<Rigidbody2D> ();
        this._trail = GetComponent<TrailRenderer> ();
        this._collider = GetComponent<CircleCollider2D> ();
        this._light = GetComponent<Light> ();
        this._body.velocity = InitialVelocity;
    }

    void OnBecameInvisible ()
    {
        if (this.isActiveAndEnabled) {
            StartCoroutine (ResetPosition ());
#if (UNITY_EDITOR || DEVELOPMENT_BUILD) && TRACE
        Debug.Log ("Comet left the screen", this);
#endif
        }
    }
    
    private IEnumerator ResetPosition ()
    {
        _trail.enabled = false;
        _light.enabled = false;
        yield return new WaitForSeconds (Random.Range (MinRestTime, MaxRestTime));

        Camera camera = Camera.main;
        float len = camera.pixelWidth + camera.pixelHeight;
        float y = Random.Range (len * MinEntryRatio, len * MaxEntryRatio);
        Vector3 start = camera.ScreenToWorldPoint (new Vector3 (camera.pixelWidth, y, 0));
        // Z seems to be the camera's Z; I don't want it as -16, I want it as 0

        transform.position = new Vector3 (start.x + _collider.radius, start.y, 0);
        _trail.enabled = true;
        _light.enabled = true;
        #if (UNITY_EDITOR || DEVELOPMENT_BUILD) && TRACE
        Debug.LogFormat("Comet re-entered the screen at {0}", transform.position);
        #endif
    }    
}
