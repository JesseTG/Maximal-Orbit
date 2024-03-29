﻿using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(DragRendering))]
[DisallowMultipleComponent]
public class PlanetManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
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
    private ScoreManager _scoreManager;
    private GameManager _gameManager;
    private DragRendering _dragRendering;
    private PlacingState _state;
    private Sun _sun;
    private int _quality;

    void Start()
    {
        _state = PlacingState.Idle;
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        _scoreManager = FindObjectOfType<ScoreManager>();
        _dragRendering = GetComponent<DragRendering>();
        _quality = QualitySettings.GetQualityLevel();
        _sun = FindObjectOfType<Sun>();
    }

    public void ClearPlanets()
    {
        GameObject[] planets = GameObject.FindGameObjectsWithTag("Planet");
        foreach (GameObject planet in planets)
        {
            if (planet.activeInHierarchy)
            {
                GameObject.Destroy(planet, .05f);
            }
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

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!this._waitingPlanet)
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
                AudioSource planetAudio = _waitingPlanet.GetComponent<AudioSource>();

                planetAudio.PlayOneShot(this.TouchSound);
                planet.OnRevolution.AddListener(_scoreManager.PlanetRevolved);
                planet.OnCrash.AddListener(_gameManager.EndGame);
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (this._waitingPlanet)
        {
            Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 distance = _placingDropOff - mouse;
            AudioSource planetAudio = _waitingPlanet.GetComponent<AudioSource>();
            planetAudio.PlayOneShot(this.ReleaseSound);

            Vector2 force = Vector2.ClampMagnitude(distance, MaxDistance);
            force *= force.magnitude;
            this._waitingPlanet.GetComponent<Rigidbody2D>().AddForce(
            force,
            ForceMode2D.Impulse
            );

            PointGravity2D gravity = _waitingPlanet.GetComponent<PointGravity2D>();
            Renderer renderer = _waitingPlanet.GetComponent<Renderer>();

            renderer.sharedMaterial = Materials[UnityEngine.Random.Range(0, Materials.Length)];
            gravity.enabled = true;

            this._waitingPlanet.layer = this.gameObject.layer;
            this._state = PlacingState.Idle;

            this.OnPlanetLaunched.Invoke(this._waitingPlanet.GetComponent<Planet>());
            this._waitingPlanet = null;

            _dragRendering.enabled = false;
        }
    }

    void OnApplicationFocus(bool focusStatus)
    {
        if (!focusStatus)
        {
            GameObject.Destroy(this._waitingPlanet);
            this._waitingPlanet = null;
            this._state = PlacingState.Idle;
            _dragRendering.enabled = false;
        }
    }
}

public enum PlacingState
{
    Idle,
    Placing
}
