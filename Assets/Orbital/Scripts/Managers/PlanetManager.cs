using UnityEngine;

[RequireComponent(typeof(DragRendering))]
[DisallowMultipleComponent]
public class PlanetManager : MonoBehaviour
{
    public PlanetAssetData[] Data;
    [Tooltip("Farthest distance the player can drag and still affect the launch force")]
    public float
        MaxDistance = 5;
    public AudioClip ReleaseSound;
    public AudioClip TouchSound;
    public PlanetLaunchedEvent OnPlanetLaunched;

    public Vector2 PlacingDropOff {
        get {
            return this._placingDropOff;
        }
    }
    
    public PlacingState State {
        get {
            return _state;
        }
    }
    
    public GameObject WaitingPlanet {
        get {
            return _waitingPlanet;
        }
    }

    private Vector2 _placingDropOff;
    private GameObject _waitingPlanet;
    private GameManager _gameManager;
    private GUIManager _gui;
    private AudioSource _audio;
    private DragRendering _dragRendering;
    private PlacingState _state;
    private int _quality;

    void Start ()
    {
        _state = PlacingState.Idle;
        _gameManager = GameObject.FindObjectOfType<GameManager> ();
        _gui = GameObject.FindObjectOfType<GUIManager> ();
        _audio = GetComponent<AudioSource> ();
        _dragRendering = GetComponent<DragRendering> ();
        _quality = QualitySettings.GetQualityLevel ();
    }
   
    // Update is called once per frame
    void Update ()
    {
        if (this._gameManager.State == GameState.Playing) {
            if (Input.GetButtonDown ("PlacePlanet")) {
                this._state = PlacingState.Placing;
                this._placingDropOff = Camera.main.ScreenToWorldPoint (Input.mousePosition);
                _dragRendering.enabled = true;

                this._waitingPlanet = GameObject.Instantiate (
                this.Data [_quality].PlanetPrefab,
                this._placingDropOff,
                Quaternion.identity
                ) as GameObject;


                Planet planet = _waitingPlanet.GetComponent<Planet> ();
                MeshFilter mf = _waitingPlanet.GetComponent<MeshFilter> ();
                MeshRenderer mr = _waitingPlanet.GetComponent<MeshRenderer> ();

                mf.sharedMesh = Data [_quality].PlanetMesh;

                _audio.PlayOneShot (this.TouchSound);
                planet.OnFirstRevolution.AddListener (_gameManager.PlanetFirstRevolved);
                planet.OnCrash.AddListener (_gameManager.EndGame);

            } else if (this._waitingPlanet && Input.GetButtonUp ("PlacePlanet")) {
                Vector2 mouse = Camera.main.ScreenToWorldPoint (Input.mousePosition);
                Vector2 distance = _placingDropOff - mouse;
                _audio.PlayOneShot (this.ReleaseSound);

                Vector2 force = Vector2.ClampMagnitude (distance, MaxDistance);
                force *= force.magnitude;
                this._waitingPlanet.GetComponent<Rigidbody2D> ().AddForce (
                force, 
                ForceMode2D.Impulse
                );

                this._waitingPlanet.GetComponent<Renderer> ().sharedMaterial =
                    Data [_quality].Materials [Random.Range (0, Data [_quality].Materials.Length)];

                this._waitingPlanet.layer = this.gameObject.layer;
                this._state = PlacingState.Idle;
                this._waitingPlanet = null;
         
                this.OnPlanetLaunched.Invoke ();

                _dragRendering.enabled = false;
            }
        }
    }

    public void ClearPlanets ()
    {
        GameObject[] planets = GameObject.FindGameObjectsWithTag ("Planet");
        foreach (GameObject planet in planets) {
            GameObject.Destroy (planet);
        }
    }

    void OnEnable ()
    {
        this.ClearPlanets ();
        this._state = PlacingState.Idle;
        this._quality = QualitySettings.GetQualityLevel ();
    }
    
    void OnDisable ()
    {
        this.ClearPlanets ();
    }
}

public enum PlacingState
{
    Idle,
    Placing
}
