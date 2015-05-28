using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SmartLocalization;
using SmartLocalization.Editor;

[RequireComponent(typeof(Text), typeof(LocalizedText))]
[DisallowMultipleComponent]
public class ScoreDisplay : MonoBehaviour
{
    private Text _text;
    private LocalizedText _localText;
    private GameManager _gameManager;

    void Start ()
    {
        _text = GetComponent<Text> ();
        _localText = GetComponent<LocalizedText> ();
        _gameManager = GameObject.FindObjectOfType<GameManager>();

        this.UpdateScore(0,0);
    }

    public void UpdateScore (int oldScore, int newScore)
    {
        _text.text = string.Format (
            LanguageManager.Instance.GetTextValue (_localText.localizedKey),
            _gameManager.Score
        );
    }

    public void OnGameStart ()
    {
        UpdateScore (0, 0);
    }
}
