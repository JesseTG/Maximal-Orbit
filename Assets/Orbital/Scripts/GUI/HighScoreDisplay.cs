using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SmartLocalization;
using SmartLocalization.Editor;

[RequireComponent(typeof(Text), typeof(LocalizedText))]
[DisallowMultipleComponent]
public class HighScoreDisplay : MonoBehaviour
{
    private ScoreManager _scoreManager;
    private Text _text;
    private LocalizedText _localized;

    // Use this for initialization
    void Start ()
    {
        _scoreManager = FindObjectOfType<ScoreManager> () as ScoreManager;
        _text = GetComponent<Text> ();
        _localized = GetComponent<LocalizedText> ();

        DisplayHighScore ();
    }

    public void DisplayHighScore ()
    {
        _text.text = string.Format (
            LanguageManager.Instance.GetTextValue (_localized.localizedKey),
            _scoreManager.HighScore
        );
    }
}
