using UnityEngine;
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
    public int Score;
    public int HighScore;

    // Events
    [Header("Events")]
    public GameStartEvent
        OnGameStart;
    public GameOverEvent OnGameOver;
    public ScoreChangedEvent OnScoreChanged;

    // Privates
    private GameObject _explosion;
    private ParticleSystem _explosionParticles;
    private AudioSource _explosionAudio;

    void Start ()
    {
        this.HighScore = PlayerPrefs.GetInt (HighScoreKey, this.HighScore);

#if DEVELOPMENT_BUILD || UNITY_EDITOR
        Debug.Log ("Loaded high score " + this.HighScore);
#endif
        this._explosion = GameObject.Instantiate (this.ExplosionPrefab) as GameObject;

        this._explosionParticles = _explosion.GetComponent<ParticleSystem> ();
        this._explosionAudio = _explosion.GetComponent<AudioSource> ();
    }

    public void UpdateScore (Planet planet)
    {
        this.Score += 1;
        this.OnScoreChanged.Invoke(this.Score - 1, this.Score);
    }

    public void CreateExplosion (Planet planet)
    {
        if (!_explosionParticles.isPlaying) {
            _explosion.transform.position = planet.gameObject.transform.position;
            _explosionParticles.Play ();
            _explosionAudio.PlayOneShot (_explosionAudio.clip);
        }
    }

    public void GameStart ()
    {
        this.Score = 0;
        _explosionParticles.Stop ();
        _explosionParticles.Clear ();
        this.State = GameState.Playing;
        this.OnGameStart.Invoke ();
    }

    public void EndGame (Planet planet)
    {
        if (this.State != GameState.Title) {
            this.State = GameState.Title;
            this.OnGameOver.Invoke ();

#if DEVELOPMENT_BUILD || UNITY_EDITOR
            Debug.LogFormat(this, "Game over! You scored {0}", this.Score);
#endif
        }
    }

    public void SaveHighScore ()
    {
        if (this.Score >= this.HighScore) {
            PlayerPrefs.SetInt (GameManager.HighScoreKey, this.Score);
            this.HighScore = this.Score;

            PlayerPrefs.Save ();
#if DEVELOPMENT_BUILD || UNITY_EDITOR
            Debug.LogFormat(this, "Saved high score of {0}", this.Score);
#endif
        }
    }
}

public enum GameState
{
    Title,
    Playing
}
