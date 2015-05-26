using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer), typeof(PointGravity2D))]
public class Planet : MonoBehaviour
{
    public float MinAxisRotationSpeed = -5;
    public float MaxAxisRotationSpeed = 5;
    public string PlanetTag = "Planet";
    public string StarTag = "Star";
    public FirstRevolutionEvent OnFirstRevolution;
    public PlanetDestroyedEvent OnCrash;
    private MeshRenderer _renderer;
    private float _total_angle;
    private int _revolutions;
    private bool _dying;
    private Rigidbody2D _body;
    private GameObject _star;

    public int Revolutions {
        get {
            return this._revolutions;
        }
    }

    private PointGravity2D _pointGravity;

    void Start ()
    {
        this._pointGravity = GetComponent<PointGravity2D> ();
        this._renderer = GetComponent<MeshRenderer> ();
        this._body = GetComponent<Rigidbody2D> ();
        this._star = GameObject.FindGameObjectWithTag (StarTag);


        _body.angularVelocity = Random.Range (MinAxisRotationSpeed, MaxAxisRotationSpeed);
    }

    void Update ()
    {
        if (!this._pointGravity.enabled && Input.GetButtonUp ("PlacePlanet")) {
            this._pointGravity.enabled = true;
        } else if (this._pointGravity.enabled) {
            Vector2 a = _body.position.normalized;
            Vector2 b = (a + _body.velocity).normalized;

            float angle = Vector2.Angle (a, b);
            this._total_angle += angle * Time.deltaTime;

            if (this._total_angle >= 360) {
                ++this._revolutions;
                this._total_angle = 0;

                if (this._revolutions == 1) {
                    this.OnFirstRevolution.Invoke (this);
                }
            }
        }
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if (!this._dying && (other.tag == this.PlanetTag || other.tag == this.StarTag)) {
            this._dying = true;
            this.OnCrash.Invoke (this);
            GameObject.Destroy (this.gameObject, .05f);
        }

    }


   

}
