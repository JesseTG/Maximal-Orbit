using UnityEngine;
using System.Collections;

public class LogoLink : MonoBehaviour {

    public string Url;

    public void OnClick()
    {
        Application.OpenURL(Url);
    }
}
