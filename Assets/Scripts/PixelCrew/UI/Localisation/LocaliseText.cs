using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocaliseText : AbstractLocalizeComponent
{
    [SerializeField] private string _key;
    [SerializeField] private bool _capitalize;

    private Text _text;

    protected override void Awake()
    {
        _text = GetComponent<Text>();
        base.Awake();
    
    }

   
    
    protected override void Localise()
    {
        var localized = LocalisationManager.I.Localise(_key);
        _text.text = _capitalize ? localized.ToUpper() : localized;
    }
}
