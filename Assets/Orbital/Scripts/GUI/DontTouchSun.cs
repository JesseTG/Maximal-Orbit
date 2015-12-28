using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class DontTouchSun : MonoBehaviour {
    private Animator _animator;
    private Sun _sun;
    private AudioSource _audioSource;
    private int _sunTouchedID;
    private int _gameInProgressID;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _sun = FindObjectOfType<Sun>();
        _audioSource = GetComponent<AudioSource>();
        _sunTouchedID = Animator.StringToHash("Sun Touched");
        _gameInProgressID = Animator.StringToHash("Game In Progress");
    }

	public void OnScreenTouched(Vector2 touch)
    {
        if (Vector2.Distance(_sun.transform.position, touch) < _sun.SafeZone)
        // If the user touched the sun...
        {
            this._animator.SetTrigger(this._sunTouchedID);
        }
    }

    public void OnGameStart()
    {
        this._animator.SetBool(_gameInProgressID, true);
    }

    public void OnGameEnd()
    {
        this._animator.SetBool(_gameInProgressID, false);
    }

    public void PlaySound()
    {
        _audioSource.Stop();
        _audioSource.Play();
    }
}
