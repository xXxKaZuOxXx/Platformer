using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogBoxController : MonoBehaviour
{
    
    [SerializeField] private GameObject _container;
    [SerializeField] private Animator _animator;

    [Space]
    [SerializeField] private float _textSpeed = 0.09f;

    private static readonly int IsOpen = Animator.StringToHash("IsOpen");

    [Header("Sounds")]
    [SerializeField] private AudioClip _typing;
    [SerializeField] private AudioClip _open;
    [SerializeField] private AudioClip _close;

    [Space]
    [SerializeField] protected DialogContent _content;
    
    [Space]

    private DialogData _data;
    private int _currentSentence;
    private UnityEvent _onComplete;
    private AudioSource _sfxSource;
    private Coroutine _typingCoroutine;

    protected Sentense CurrentSentence => _data.Sentences[_currentSentence];
    protected virtual DialogContent CurrentContent => _content;

    private void Start()
    {
        _sfxSource = AudioUtils.FindSfxSource();
    }
    public void ShowDialog(DialogData data, UnityEvent onComplete)
    {
        _onComplete =  onComplete;
        _data = data;
        _currentSentence = 0;
        CurrentContent.Text.text = string.Empty;

        _container.SetActive(true);
        _sfxSource.PlayOneShot(_open);
        _animator.SetBool(IsOpen, true);
    }
    
    public void OnSkip()
    {
        if (_typingCoroutine == null)
            return;

        StopTypeAnimation();
        CurrentContent.Text.text = _data.Sentences[_currentSentence].Value;
    }

    private void StopTypeAnimation()
    {
        if(_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);
        _typingCoroutine = null;

    }

    public void OnContinue()
    {
        StopTypeAnimation();
        _currentSentence++;

        var isDialogComplete = _currentSentence >= _data.Sentences.Length;
        if(isDialogComplete)
        {
            HideDialogBox();
            _onComplete?.Invoke();
        }
        else
        {
            OnStartDialogAnimation();
        }
    }

    private void HideDialogBox()
    {
        _animator.SetBool(IsOpen, false);
        _sfxSource?.PlayOneShot(_close);
    }

    protected virtual void OnStartDialogAnimation()
    {
        _typingCoroutine = StartCoroutine(TypeDialogText());
    }

    private IEnumerator TypeDialogText()
    {
        CurrentContent.Text.text = string.Empty;
        var sentenses = CurrentSentence;
        CurrentContent.TrySetIcon(sentenses.Icon);
        foreach (var sent in sentenses.Value)
        {
            CurrentContent.Text.text += sent;
            _sfxSource?.PlayOneShot(_typing);
            yield return new WaitForSeconds(_textSpeed);
        }
        _typingCoroutine = null;
    }

    private void OnCloseAnimationComplete()
    {

    }



    

}
