using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

/// <summary>
/// Manages the game state
/// </summary>
[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    public string PlanetTag = "Planet";
    public GameObject ExplosionPrefab;
    public GameState State = GameState.Title;
    public int GamesPerAd = 5;

    // Events
    [Header("Events")]
    //

    public UnityEvent
        OnGameStart;
    public UnityEvent OnGameOver;

    // Privates
    private GameObject _explosion;
    private ParticleSystem _explosionParticles;
    private AudioSource _explosionAudio;
    private int _gamesThisSession;

    void Start()
    {
        _explosion = GameObject.Instantiate(ExplosionPrefab);

        _explosionParticles = _explosion.GetComponent<ParticleSystem>();
        _explosionAudio = _explosion.GetComponent<AudioSource>();
    }

    public void GameStart()
    {

        _explosionParticles.Stop();
        _explosionParticles.Clear();
        this.State = GameState.Playing;
        this.OnGameStart.Invoke();
    }

    public void EndGame(Planet planet)
    {
        if (this.State != GameState.Title)
        {
            if (!_explosionParticles.isPlaying)
            // If the explosion hasn't already been triggered...
            {
                _explosion.transform.position = planet.transform.position;
                _explosionParticles.Play();
                _explosionAudio.PlayOneShot(_explosionAudio.clip);
            }

            this.State = GameState.Title;
            this._gamesThisSession++;
            this.OnGameOver.Invoke();

#if UNITY_ADS
            if (this._gamesThisSession % this.GamesPerAd == 0 && Advertisement.IsReady())
                // Every nth game (where n == GamesPerAd)...
            {
                Advertisement.Show();
            }
#endif
        }
    }


}

public enum GameState
{
    Title,
    Playing
}
