using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocilizeDialog : MonoBehaviour
{
    [SerializeField] private string[] _keys;
    [SerializeField] private bool _capitalize;
    [SerializeField] private DialogDef[] _dialogDefs;
    private ShowDialogComponent _text;

    private void Awake()
    {
        _text = GetComponent<ShowDialogComponent>();

        LocalisationManager.I.OnLocalChanged += OnLocalChanged;
        
    }
    private void Start()
    {
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
        for (int i = 0; i < _keys.Length; i++)
        {
            if (_text._mode == ShowDialogComponent.Mode.Bound)
            {
                int indexToChange = Array.IndexOf(_text._bound._localisationKeys, _keys[i]);
                if (indexToChange != -1)
                {
                    var localized = LocalisationManager.I.Localise(_keys[i]);
                    _text.Data._sentences[indexToChange]._value = _capitalize ? localized.ToUpper() : localized;
                }
                else
                {
                    _text.Data._sentences[0]._value = "ERROR";
                }
            }
            else
            {
                for (int k = 0; k < _dialogDefs.Length; k++)
                {
                    int indexToChange = Array.IndexOf(_dialogDefs[k].Data._localisationKeys, _keys[i]);
                    if (indexToChange != -1)
                    {
                        var localized = LocalisationManager.I.Localise(_keys[i]);
                        _dialogDefs[k].Data._sentences[indexToChange]._value = _capitalize ? localized.ToUpper() : localized;
                    }
                }
                
                
            }
            
            
        }
        
        //var localized = LocalisationManager.I.Localise(_keys);
       
    }
}
