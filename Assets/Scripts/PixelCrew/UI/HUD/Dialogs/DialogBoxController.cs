﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogBoxController : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private GameObject _container;
    [SerializeField] private Animator _animator;

    [Space]
    [SerializeField] private float _textSpeed = 0.09f;

    private static readonly int IsOpen = Animator.StringToHash("IsOpen");

    [Header("Sounds")]
    [SerializeField] private AudioClip _typing;
    [SerializeField] private AudioClip _open;
    [SerializeField] private AudioClip _close;

    private DialogData _data;
    private int _currentSentence;
    private AudioSource _sfxSource;
    private Coroutine _typingCoroutine;

    private void Start()
    {
        _sfxSource = AudioUtils.FindSfxSource();
    }
    public void ShowDialog(DialogData data)
    {
        _data = data;
        _currentSentence = 0;
        _text.text = string.Empty;

        _container.SetActive(true);
        _sfxSource.PlayOneShot(_open);
        _animator.SetBool(IsOpen, true);
    }
    public void OnSkip()
    {
        if (_typingCoroutine == null)
            return;

        StopTypeAnimation();
        _text.text = _data.Sentences[_currentSentence];
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

    private void OnStartDialogAnimation()
    {
        _typingCoroutine = StartCoroutine(TypeDialogText());
    }

    private IEnumerator TypeDialogText()
    {
        _text.text = string.Empty;
        var sentenses = _data.Sentences[_currentSentence];
        foreach (var sent in sentenses)
        {
            _text.text += sent;
            _sfxSource?.PlayOneShot(_typing);
            yield return new WaitForSeconds(_textSpeed);
        }
        _typingCoroutine = null;
    }

    private void OnCloseAnimationComplete()
    {

    }



    

}
