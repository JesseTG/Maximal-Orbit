using UnityEngine;
using UnityEngine.UI;
using GameUp;
using System.Collections;
using SmartLocalization;
using SmartLocalization.Editor;

[RequireComponent(typeof(Text))]
[DisallowMultipleComponent]
public class HighScoreDisplay : MonoBehaviour
{
    private ScoreManager _scoreManager;
    private Text _text;

    public string LeaderboardKey;
    public string HighScoreUnknown = "?";

    // Use this for initialization
    void Start()
    {
        _scoreManager = FindObjectOfType<ScoreManager>() as ScoreManager;
        _text = GetComponent<Text>();
        
        if (this.LeaderboardKey == "")
        {
            DisplayHighScore("", _scoreManager.HighScore);
        }
        else
        {
            this._text.text = this.HighScoreUnknown;
        }
    }

    public void DisplayHighScore(string key, long score)
    {
        if (key == this.LeaderboardKey)
        {
            _text.text = string.Format("{0}", score);
        }
    }

    public void OnLeaderboardNotFound()
    {
        _text.text = this.HighScoreUnknown;
    }
}
