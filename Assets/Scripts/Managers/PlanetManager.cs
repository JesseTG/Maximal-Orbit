using UnityEngine;
using System;

public class PlanetManager : MonoBehaviour
{
    [Tooltip("The planet that will be created")]
    public GameObject
        PlanetPrefab;
    [Tooltip("Farthest distance the player can drag and still affect the launch force")]
    public float
        MaxDistance = 5;
    [Tooltip("The layer that planets should be put on when released.  Only pick one!")]
    public LayerMask
        ActiveLayer;
    public AudioClip ReleaseSound;
    public AudioClip TouchSound;
    public PlanetLaunchedEvent OnPlanetLaunched;
    [HideInInspector]
    public PlacingState
        State;
    private Vector3 _placingDropOff;
    private GameObject _currentPlanet;
    private GameManager _gameManager;
    private GUIManager _gui;

    public Vector3 PlacingDropOff {
        get {
            return this._placingDropOff;
        }
    }

    // Use this for initialization
    void Start ()
    {
        this.State = PlacingState.Idle;
        this._gameManager = GameObject.FindObjectOfType<GameManager> ();
        this._gui = GameObject.FindObjectOfType<GUIManager> ();
    }
    
    // Update is called once per frame
    void Update ()
    {
        if (this._gameManager.State == GameState.Playing) {
            if (Input.GetButtonDown ("PlacePlanet")) {
                this.State = PlacingState.Placing;
                this._placingDropOff = Camera.main.ScreenToWorldPoint (Input.mousePosition);
                this._placingDropOff.z = 0;

                this._currentPlanet = GameObject.Instantiate (
                this.PlanetPrefab,
                this._placingDropOff,
                Quaternion.identity
                ) as GameObject;


                Planet planet = this._currentPlanet.GetComponent<Planet> ();
                this.audio.PlayOneShot (this.TouchSound);
                planet.OnFirstRevolution.AddListener (this._gameManager.UpdateScore);
                planet.OnFirstRevolution.AddListener (this._gui.UpdateScore);
                planet.OnCrash.AddListener (this._gameManager.CreateExplosion);
                planet.OnCrash.AddListener (this._gameManager.EndGame);
            } else if (this._currentPlanet && Input.GetButtonUp ("PlacePlanet")) {
                Vector2 mouse = Camera.main.ScreenToWorldPoint (Input.mousePosition);
                Vector2 distance = new Vector2 (this._placingDropOff.x, this._placingDropOff.y) - mouse;
                this.audio.PlayOneShot (this.ReleaseSound);

                Vector2 force = Vector2.ClampMagnitude (distance, this.MaxDistance);
                force *= force.magnitude;
                this._currentPlanet.rigidbody2D.AddForce (
                force, 
                ForceMode2D.Impulse
                );

                this._currentPlanet.GetComponent<Planet>().Release();
                
                this._currentPlanet.layer = (int)Mathf.Log (this.ActiveLayer.value, 2);
               
                this._placingDropOff = new Vector3 (float.NaN, float.NaN, float.NaN);
                this.State = PlacingState.Idle;
                this._currentPlanet = null;

                this.OnPlanetLaunched.Invoke ();
            }
        }
    }

    public void ClearPlanets ()
    {
        GameObject[] planets = GameObject.FindGameObjectsWithTag ("Planet");
        foreach (GameObject planet in planets) {
            GameObject.Destroy (planet);
        }

        this.State = PlacingState.Idle;
    }
}

public enum PlacingState
{
    Idle,
    Placing
}
