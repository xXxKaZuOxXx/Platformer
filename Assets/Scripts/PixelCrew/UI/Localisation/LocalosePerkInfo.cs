using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LocalosePerkInfo : AbstractLocalizeComponent
{
    [SerializeField] private string[] _keys;
    [SerializeField] private PerkRepository _perkRepository;

    private Text _text;

    protected override void Awake()
    {
        _text = GetComponent<Text>();
        base.Awake();

    }
    protected override void Localise()
    {
        for (int i = 0; i < _keys.Length; i++)
        {
            for (int k = 0; k < _perkRepository.Collection.Length; k++)
            {
                if (_perkRepository.Collection[k].Id == _keys[i])
                {
                    var localized = LocalisationManager.I.Localise(_keys[i]);
                   _text.text = localized;
                }
            }
        }
    }
}
