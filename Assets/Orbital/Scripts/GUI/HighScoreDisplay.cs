using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SmartLocalization;
using SmartLocalization.Editor;

[RequireComponent(typeof(Text), typeof(LocalizedText))]
public class HighScoreDisplay : MonoBehaviour
{
    private GameManager _gameManager;
    private Text _text;
    private LocalizedText _localized;

    // Use this for initialization
    void Start ()
    {
        this._gameManager = GameObject.FindObjectOfType<GameManager> () as GameManager;
        this._text = this.GetComponent<Text> ();
        this._localized = this.GetComponent<LocalizedText> ();

        this.DisplayHighScore();
    }

    public void DisplayHighScore ()
    {
        this._text.text = string.Format (
            LanguageManager.Instance.GetTextValue (this._localized.localizedKey),
            this._gameManager.HighScore
        );
    }
}
