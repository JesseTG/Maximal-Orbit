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

    void Start ()
    {
        _text = GetComponent<Text> ();
        _localText = GetComponent<LocalizedText> ();

        this.UpdateScore(0);
    }

    public void UpdateScore (int score)
    {
        _text.text = string.Format (
            LanguageManager.Instance.GetTextValue (_localText.localizedKey),
            score
        );
    }

    public void OnGameStart ()
    {
        UpdateScore(0);
    }
}
