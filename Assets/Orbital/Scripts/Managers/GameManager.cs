using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Manages the game state
/// </summary>
[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    public const string HighScoreKey = "Best Score";
    public string PlanetTag = "Planet";
    public GameObject ExplosionPrefab;
    public GameState State = GameState.Title;

    public int HighScore {
        get {
            return _highScore;
        }
    }

    public int Score {
        get {
            return _score;
        }
    }

    private int _highScore;
    private int _score;

    // Events
    [Header("Events")]
    //

    public UnityEvent
        OnGameStart;
    public UnityEvent OnGameOver;
    public IntEvent ScoreChanged;

    // Privates
    private GameObject _explosion;
    private ParticleSystem _explosionParticles;
    private AudioSource _explosionAudio;

    void Start ()
    {
        string id = SystemInfo.deviceUniqueIdentifier;
        ZPlayerPrefs.Initialize (id, id);
#if DEVELOPMENT_BUILD || UNITY_EDITOR
        Debug.LogFormat (this, "Initialized ZPlayerPrefs with device ID {0}", id);
#endif
        _highScore = ZPlayerPrefs.GetInt (HighScoreKey);

#if DEVELOPMENT_BUILD || UNITY_EDITOR
        Debug.LogFormat (this, "Loaded high score {0}", HighScore);
#endif
        _explosion = GameObject.Instantiate (ExplosionPrefab);

        _explosionParticles = _explosion.GetComponent<ParticleSystem> ();
        _explosionAudio = _explosion.GetComponent<AudioSource> ();
    }

    public void PlanetFirstRevolved (Planet planet)
    {
        if (planet.Revolutions == 1)
        {
            this._score++;
            this.ScoreChanged.Invoke(this._score);
        }
    }

    public void GameStart ()
    {
        this._score = 0;
        _explosionParticles.Clear ();
        this.State = GameState.Playing;
        this.OnGameStart.Invoke ();
    }

    public void EndGame (Planet planet)
    {
        if (this.State != GameState.Title) {
            if (!_explosionParticles.isPlaying) {
                _explosion.transform.position = planet.gameObject.transform.position;
                _explosionParticles.Play ();
                _explosionAudio.PlayOneShot (_explosionAudio.clip);
            }

            this.State = GameState.Title;
            this.OnGameOver.Invoke ();

#if DEVELOPMENT_BUILD || UNITY_EDITOR
            Debug.LogFormat(this, "Game over! You scored {0}", Score);
#endif
        }
    }

    public void SaveHighScore ()
    {
        if (this.Score >= this.HighScore) {
            ZPlayerPrefs.SetInt (GameManager.HighScoreKey, Score);
            this._highScore = Score;

            ZPlayerPrefs.Save ();

            #if DEVELOPMENT_BUILD || UNITY_EDITOR
            Debug.LogFormat(this, "Saved high score of {0}", Score);
            #endif
        }
    }
}

public enum GameState
{
    Title,
    Playing
}
