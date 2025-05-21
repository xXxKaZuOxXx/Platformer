using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocaliseText : MonoBehaviour
{
    [SerializeField] private string _key;
    [SerializeField] private bool _capitalize;

    private Text _text;

    private void Awake()
    {
        _text = GetComponent<Text>();

        LocalisationManager.I.OnLocalChanged += OnLocalChanged;
        Localise();
    }

    private void OnLocalChanged()
    {
        Localise();
    }
    private void OnDestroy()
    {
        LocalisationManager.I.OnLocalChanged -= OnLocalChanged;
    }
    private void Localise()
    {
        var localized = LocalisationManager.I.Localise(_key);
        _text.text = _capitalize ? localized.ToUpper() : localized;
    }
}
