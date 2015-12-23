using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(DragRendering))]
[DisallowMultipleComponent]
public class PlanetManager : MonoBehaviour
{
    public GameObject PlanetPrefab;
    public Material[] Materials;
    [Tooltip("Farthest distance the player can drag and still affect the launch force")]
    public float
        MaxDistance = 5;

    public AudioClip ReleaseSound;
    public AudioClip TouchSound;
    public PlanetEvent OnPlanetLaunched;
    public Vector2Event OnScreenTouched;

    public Vector2 PlacingDropOff
    {
        get
        {
            return this._placingDropOff;
        }
    }

    public PlacingState State
    {
        get
        {
            return _state;
        }
    }

    public GameObject WaitingPlanet
    {
        get
        {
            return _waitingPlanet;
        }
    }

    private Vector2 _placingDropOff;
    private GameObject _waitingPlanet;
    private GameManager _gameManager;
    private AudioSource _audio;
    private DragRendering _dragRendering;
    private PlacingState _state;
    private Sun _sun;
    private int _quality;

    void Start()
    {
        _state = PlacingState.Idle;
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        _audio = GetComponent<AudioSource>();
        _dragRendering = GetComponent<DragRendering>();
        _quality = QualitySettings.GetQualityLevel();
        _sun = FindObjectOfType<Sun>();
    }

    void Update()
    {
        if (Input.GetButtonDown("PlacePlanet"))
        // If the user touches the screen or clicks the mouse...
        {
            Vector3 touch = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            this.OnScreenTouched.Invoke(touch);

            if (Vector2.Distance(_sun.transform.position, touch) > _sun.SafeZone)
            // If the player didn't touch too close to the sun...
            {
                this._state = PlacingState.Placing;
                this._placingDropOff = touch;
                _dragRendering.enabled = true;

                this._waitingPlanet = GameObject.Instantiate(
                this.PlanetPrefab,
                this._placingDropOff,
                Quaternion.identity
                ) as GameObject;


                Planet planet = _waitingPlanet.GetComponent<Planet>();
                MeshFilter mf = _waitingPlanet.GetComponent<MeshFilter>();
                MeshRenderer mr = _waitingPlanet.GetComponent<MeshRenderer>();


                _audio.PlayOneShot(this.TouchSound);
                planet.OnRevolution.AddListener(_gameManager.PlanetFirstRevolved);
                planet.OnCrash.AddListener(_gameManager.EndGame);
            }


        }
        else if (this._waitingPlanet && Input.GetButtonUp("PlacePlanet"))
        {
            Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 distance = _placingDropOff - mouse;
            _audio.PlayOneShot(this.ReleaseSound);

            Vector2 force = Vector2.ClampMagnitude(distance, MaxDistance);
            force *= force.magnitude;
            this._waitingPlanet.GetComponent<Rigidbody2D>().AddForce(
            force,
            ForceMode2D.Impulse
            );

            this._waitingPlanet.GetComponent<Renderer>().sharedMaterial =
                Materials[Random.Range(0, Materials.Length)];

            this._waitingPlanet.layer = this.gameObject.layer;
            this._state = PlacingState.Idle;

            this.OnPlanetLaunched.Invoke(this._waitingPlanet.GetComponent<Planet>());
            this._waitingPlanet = null;

            _dragRendering.enabled = false;
        }

    }

    public void ClearPlanets()
    {
        GameObject[] planets = GameObject.FindGameObjectsWithTag("Planet");
        foreach (GameObject planet in planets)
        {
            GameObject.Destroy(planet);
        }
    }

    void OnEnable()
    {
        this.ClearPlanets();
        this._state = PlacingState.Idle;
        this._quality = QualitySettings.GetQualityLevel();
    }

    void OnDisable()
    {
        this.ClearPlanets();
    }
}

public enum PlacingState
{
    Idle,
    Placing
}
