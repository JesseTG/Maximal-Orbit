using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer), typeof(PointGravity2D))]
public class Planet : MonoBehaviour
{
    public float MinAxisRotationSpeed = -5;
    public float MaxAxisRotationSpeed = 5;
    public string
        PlanetTag = "Planet";
    public string StarTag = "Star";
    [Tooltip("The materials a planet can be rendered with.  Randomly chosen on creation")]
    public Material[]
        Materials;
    [Tooltip("The sun at the center of the game")]
    public GameObject
        Star;
    public FirstRevolutionEvent OnFirstRevolution;
    public PlanetDestroyedEvent OnCrash;
    private float _total_angle;
    private int _revolutions;
    private bool _dying;

    public int Revolutions {
        get {
            return this._revolutions;
        }
    }

    private PointGravity2D _pointGravity;

    void Start ()
    {
        this._pointGravity = this.GetComponent<PointGravity2D> ();
        this.Star = GameObject.FindGameObjectWithTag (StarTag);
        this.rigidbody2D.angularVelocity = Random.Range (this.MinAxisRotationSpeed, this.MaxAxisRotationSpeed);
    }

    public void Release() {
        
        this.renderer.sharedMaterial = this.Materials [Random.Range (0, this.Materials.Length)];
    }

    void Update ()
    {
        if (!this._pointGravity.enabled && Input.GetButtonUp ("PlacePlanet")) {
            this._pointGravity.enabled = true;
        } else if (this._pointGravity.enabled) {
            Vector2 a = this.rigidbody2D.position.normalized;
            Vector2 b = (a + this.rigidbody2D.velocity).normalized;

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
