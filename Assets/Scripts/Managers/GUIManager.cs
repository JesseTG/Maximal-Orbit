using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIManager : MonoBehaviour
{
    public string HighScoreText = "Best: ";
    private GameManager _gameManager;
    private GameObject _title;
    private Text _score;
    private Text _highScore;

    void Start ()
    {
        this._title = GameObject.Find ("Title");
        this._score = GameObject.Find ("Score").GetComponent<Text> ();
        this._highScore = GameObject.Find ("High Score").GetComponent<Text> ();
        this._gameManager = GameObject.FindObjectOfType<GameManager> () as GameManager;

        this._highScore.text = this.HighScoreText + this._gameManager.HighScore;
    }

    public void OnGameStart ()
    {
        this._title.SetActive (false);
        this._score.text = "0";
    }

    public void OnGameEnd ()
    {
        this._title.SetActive (true);

        if (this._gameManager.Score >= this._gameManager.HighScore) {
            this._highScore.text = this.HighScoreText + this._gameManager.Score;
        }
    }

    public void UpdateScore (Planet planet)
    {
        int score = this._gameManager.Score;
        this._score.text = score.ToString ();
    }
}
