using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup), typeof(Canvas), typeof(Animator))]
public class Tutorial : MonoBehaviour
{
    private Animator _animator;
    private CanvasGroup _canvasGroup;
    private Canvas _canvas;
    private int _gameInProgressID;
    private int _gameLoadedID;
    private int _firstRevolutionID;
    private int _secondRevolutionID;

    void Start()
    {
        this._canvasGroup = GetComponent<CanvasGroup>();
        this._canvas = GetComponent<Canvas>();
        this._animator = GetComponent<Animator>();
        this._gameInProgressID = Animator.StringToHash("Game In Progress");
        this._gameLoadedID = Animator.StringToHash("Game Loaded");
        this._firstRevolutionID = Animator.StringToHash("First Revolution");
        this._secondRevolutionID = Animator.StringToHash("Second Revolution");

        _animator.SetTrigger(_gameLoadedID);
    }

    public void OnPlanetLaunched(Planet planet)
    {
        planet.OnRevolution.AddListener(this.OnPlanetRevolution);
    }

    public void OnPlanetRevolution(Planet planet)
    {
        if (planet.Revolutions == 1)
        // If this is the planet's first revolution...
        {
            Debug.Log("Planet revolved first time", planet);
            _animator.SetTrigger(_firstRevolutionID);
        }
        else if (planet.Revolutions == 2)
        {
            _animator.SetTrigger(_secondRevolutionID);
            planet.OnRevolution.RemoveListener(this.OnPlanetRevolution);
        }
    }

    public void OnGameStart()
    {
        _canvas.enabled = true;
        _animator.SetBool(_gameInProgressID, true);
    }

    public void OnGameEnd()
    {
        _canvas.enabled = false;
        _animator.SetBool(_gameInProgressID, false);
    }
}
