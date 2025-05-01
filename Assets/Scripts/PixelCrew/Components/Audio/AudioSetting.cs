using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSetting : MonoBehaviour
{
    [SerializeField] private SoundSetting _mode;
    private AudioSource _source;
    private FloatPersistentProperty _model;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
        _model = FindProperty();
        _model.OnChanged += OnSoundSettingChaged;
        OnSoundSettingChaged(_model.Value, _model.Value);

    }

    private void OnSoundSettingChaged(float newValue, float oldValue)
    {
        _source.volume = newValue;
    }

    private FloatPersistentProperty FindProperty()
    {
        switch (_mode)
        {
            case SoundSetting.Music:
                return GameSettings.I.Music;
            case SoundSetting.Sfx:
                return GameSettings.I.Sfx;
        }

        throw new ArgumentException("undefined mode");
    }
    private void OnDestroy()
    {
        _model.OnChanged -= OnSoundSettingChaged;
    }
}
