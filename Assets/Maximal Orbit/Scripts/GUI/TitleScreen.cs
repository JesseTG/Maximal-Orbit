using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup), typeof(Animator))]
public class TitleScreen : MonoBehaviour {
    private Animator _animator;
    private CanvasGroup _canvasGroup;
    private int _gameInProgressID;
    private int _gameLoadedID;

  	void Start () {
        this._canvasGroup = GetComponent<CanvasGroup>();
        this._animator = GetComponent<Animator>();
        this._gameLoadedID = Animator.StringToHash("Game Loaded");
        this._gameInProgressID = Animator.StringToHash("Game In Progress");

        _animator.SetBool (_gameLoadedID, true);
	}
	
    public void OnGameStart() {
        _canvasGroup.interactable = false;
        _animator.SetBool (_gameInProgressID, true);
    }

    public void OnGameEnd() {
        _canvasGroup.interactable = true;
        _animator.SetBool (_gameInProgressID, false);
    }
}
