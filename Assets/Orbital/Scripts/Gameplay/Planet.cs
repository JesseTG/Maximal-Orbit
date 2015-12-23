using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer), typeof(PointGravity2D))]
[DisallowMultipleComponent]
public class Planet : MonoBehaviour
{
    public float MinAxisRotationSpeed = -5;
    public float MaxAxisRotationSpeed = 5;
    public string PlanetTag = "Planet";
    public string StarTag = "Star";
    public PlanetEvent OnRevolution;
    public PlanetEvent OnCrash;


    public int Revolutions {
        get {
            return this._revolutions;
        }
    }

    private MeshRenderer _renderer;
    private float _total_angle;
    private int _revolutions;
    private bool _dying;
    private Rigidbody2D _body;
    private GameObject _star;
    private PointGravity2D _pointGravity;
    private AudioSource _audioSource;
    private ParticleSystem _particleSystem;

    void Start ()
    {
        _pointGravity = GetComponent<PointGravity2D> ();
        _renderer = GetComponent<MeshRenderer> ();
        _body = GetComponent<Rigidbody2D> ();
        _star = GameObject.FindObjectOfType<Sun>().gameObject;
        _audioSource = GetComponent<AudioSource>();
        _particleSystem = GetComponent<ParticleSystem>();

        _body.angularVelocity = Random.Range (MinAxisRotationSpeed, MaxAxisRotationSpeed);
        this.OnRevolution.AddListener(this.OnFirstOrbit);
    }

    void Update ()
    {
        if (!this._pointGravity.enabled && Input.GetButtonUp ("PlacePlanet")) {
            // If this planet was being prepped for launch, then launched...
            this._pointGravity.enabled = true;

        } else if (this._pointGravity.enabled) {
            // If this planet is active...
            Vector2 a = _body.position.normalized;
            Vector2 b = (a + _body.velocity).normalized;

            float angle = Vector2.Angle (a, b);
            this._total_angle += angle * Time.deltaTime;

            if (this._total_angle >= 360) {
                // If this planet has made a revolution...
                ++this._revolutions;
                this._total_angle = 0;
                this.OnRevolution.Invoke (this);
            }
        }
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if (!this._dying && (other.tag == this.PlanetTag || other.tag == this.StarTag)) {
            // If this planet hasn't exploded, and it just hit the star or another planet...
            this._dying = true;
            this.OnCrash.Invoke (this);
            GameObject.Destroy (this.gameObject, .05f);
        }
    }

    public void OnFirstOrbit(Planet planet)
    {
        if (this._revolutions == 1)
            // If this is this planet's first orbit...
        {
            this._audioSource.Play();
            this._particleSystem.Play();
            this.OnRevolution.RemoveListener(this.OnFirstOrbit);
        }
    }
}
