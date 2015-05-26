using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SmartLocalization;
using SmartLocalization.Editor;

[RequireComponent(typeof(Text), typeof(LocalizedText))]
public class ScoreDisplay : MonoBehaviour
{
    private Text _text;
    private LocalizedText _localText;

    // Use this for initialization
    void Start ()
    {
        this._text = this.GetComponent<Text> ();
        this._localText = this.GetComponent<LocalizedText> ();
        this.UpdateScore(0, 0);
    }

    public void UpdateScore (int oldScore, int newScore)
    {
        this._text.text = string.Format (
            LanguageManager.Instance.GetTextValue (_localText.localizedKey),
            newScore
        );
    }

    public void OnGameStart() {
        this.UpdateScore(0, 0);
    }
}
