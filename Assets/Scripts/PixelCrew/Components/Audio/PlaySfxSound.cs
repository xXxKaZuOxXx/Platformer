﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySfxSound : MonoBehaviour
{
    [SerializeField] private AudioClip _clip;
    private AudioSource _source;
    private void Play()
    {
        if(_source == null)
        {
            _source = AudioUtils.FindSfxSource();
        }
        _source.PlayOneShot(_clip);
    }
}
