﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsWindow : AnimatedWindow
{
    [SerializeField] AudioSettingsWidget _music;
    [SerializeField] AudioSettingsWidget _sfx;
    protected override void Start()
    {
        base.Start();
        _music.SetModel(GameSettings.I.Music);
        _sfx.SetModel(GameSettings.I.Sfx);
    }
}
