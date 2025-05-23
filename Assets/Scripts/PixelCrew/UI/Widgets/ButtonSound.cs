﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private AudioClip _audioClip;
    
    private AudioSource _source;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_source == null)
            _source = AudioUtils.FindSfxSource();

        _source.PlayOneShot(_audioClip);
    }
}
