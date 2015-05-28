using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SmartLocalization;
using SmartLocalization.Editor;

[RequireComponent(typeof(Text), typeof(LocalizedText))]
[DisallowMultipleComponent]
public class HighScoreDisplay : MonoBehaviour
{
    private GameManager _gameManager;
    private Text _text;
    private LocalizedText _localized;

    // Use this for initialization
    void Start ()
    {
        _gameManager = GameObject.FindObjectOfType<GameManager> () as GameManager;
        _text = GetComponent<Text> ();
        _localized = GetComponent<LocalizedText> ();

        DisplayHighScore ();
    }

    public void DisplayHighScore ()
    {
        _text.text = string.Format (
            LanguageManager.Instance.GetTextValue (_localized.localizedKey),
            _gameManager.HighScore
        );
    }
}
