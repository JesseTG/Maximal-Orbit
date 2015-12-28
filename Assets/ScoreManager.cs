using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public string HighScoreKey = "Best Score";

    public int HighScore
    {
        get
        {
            return _highScore;
        }
    }

    public int Score
    {
        get
        {
            return _score;
        }
    }

    public IntEvent ScoreChanged;

    private int _highScore;
    private int _score;

    void Start()
    {
        string id = SystemInfo.deviceUniqueIdentifier;
        ZPlayerPrefs.Initialize(id, id);
#if DEVELOPMENT_BUILD || UNITY_EDITOR
        Debug.LogFormat(this, "Initialized ZPlayerPrefs with device ID {0}", id);
#endif
        _highScore = ZPlayerPrefs.GetInt(HighScoreKey);

#if DEVELOPMENT_BUILD || UNITY_EDITOR
        Debug.LogFormat(this, "Loaded high score {0}", HighScore);
#endif
    }

    public void PlanetRevolved(Planet planet)
    {
        if (planet.Revolutions == 1)
        {
            this._score++;
            this.ScoreChanged.Invoke(this._score);
        }
    }

    public void OnGameStart()
    {
        this._score = 0;
        this.ScoreChanged.Invoke(this._score);
    }

    public void OnGameEnd()
    {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
        Debug.LogFormat(this, "Game over! You scored {0}", Score);
#endif

        if (this.Score >= this.HighScore)
        {
            ZPlayerPrefs.SetInt(this.HighScoreKey, Score);
            this._highScore = Score;

            ZPlayerPrefs.Save();

#if DEVELOPMENT_BUILD || UNITY_EDITOR
            Debug.LogFormat(this, "Saved high score of {0}", Score);
#endif
        }
    }
}
