using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SmartLocalization;
using SmartLocalization.Editor;

[RequireComponent(typeof(LocalizedText), typeof(Text))]
public class QualityButton : QualityAdjuster
{
    public string[] QualityKeys;
    private LocalizedText _localized;
    private Text _text;

    void Start ()
    {
        this._localized = this.GetComponent<LocalizedText> ();
        this._text = this.GetComponent<Text> ();
        this.SetQuality(QualitySettings.GetQualityLevel());
    }
    
    public override void SetQuality (int index)
    {
        this._text.text = string.Format (
            LanguageManager.Instance.GetTextValue (this._localized.localizedKey),
            LanguageManager.Instance.GetTextValue (QualityKeys[index])
        );
    }

}
