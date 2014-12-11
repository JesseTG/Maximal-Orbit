using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Manages the game state
/// </summary>
public class GameManager : MonoBehaviour
{
    public const string HighScoreKey = "hi-score";
    public string PlanetTag = "Planet";
    public GameObject ExplosionPrefab;
    public GameState State = GameState.Title;
    public int Score;
    public int HighScore;
    public GameStartEvent OnGameStart;
    public GameOverEvent OnGameOver;

    private GameObject _explosion;

    void Start ()
    {
        this.HighScore = PlayerPrefs.GetInt (HighScoreKey, this.HighScore);
        Debug.Log ("Loaded high score " + this.HighScore);
        this._explosion = GameObject.Instantiate (this.ExplosionPrefab) as GameObject;
    }

    public void UpdateScore (Planet planet)
    {
        this.Score += 1;
    }

    public void CreateExplosion (Planet planet)
    {
        if (!this._explosion.particleSystem.isPlaying) {
            this._explosion.transform.position = planet.gameObject.transform.position;
            this._explosion.particleSystem.Play ();
            this._explosion.audio.PlayOneShot (this._explosion.audio.clip);
        }
    }

    public void GameStart() {
        this.Score = 0;
        this._explosion.particleSystem.Stop ();
        this._explosion.particleSystem.Clear();
    }

    public void EndGame (Planet planet)
    {
        if (this.State != GameState.Title) {
            this.State = GameState.Title;
            Debug.Log ("Game over!");
            this.OnGameOver.Invoke ();
        }
    }

    public void SaveHighScore ()
    {
        if (this.Score >= this.HighScore) {
            Debug.Log("Saving high score of " + this.Score);
            PlayerPrefs.SetInt (GameManager.HighScoreKey, this.Score);
            this.HighScore = this.Score;

            PlayerPrefs.Save ();
        }
    }

    void Update ()
    {
        if (this.State == GameState.Title && Input.GetButtonDown ("GameStart")) {
            this.State = GameState.Playing;
            this.OnGameStart.Invoke ();
        }
    }
}

public enum GameState
{
    Title,
    Playing
}
