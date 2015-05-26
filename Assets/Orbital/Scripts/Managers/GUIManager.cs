using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIManager : MonoBehaviour
{
    private GameManager _gameManager;
    private GameObject _title;
    private Text _score;

    void Start ()
    {
        this._title = GameObject.Find ("Title");
        this._score = GameObject.Find ("Score").GetComponent<Text> ();
        this._gameManager = GameObject.FindObjectOfType<GameManager> () as GameManager;
    }
}
