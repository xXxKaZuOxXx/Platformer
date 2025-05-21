using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalisationManager
{
    public readonly static LocalisationManager I;

    private StringPersistentProperty _localKey = new StringPersistentProperty("en", "localisation/current");
    private Dictionary<string, string> _localisation;

    public string LocalKey => _localKey.Value;

    public event Action OnLocalChanged;
    static LocalisationManager()
    {
        I = new LocalisationManager();
    }
    public LocalisationManager()
    {
        LoadLocal(_localKey.Value);
    }

    private void LoadLocal(string localToLoad)
    {
        var def = Resources.Load<LocalDef>($"Locales/{localToLoad}");
        _localisation = def.GetData();
        _localKey.Value = localToLoad;
        OnLocalChanged?.Invoke();
    }

    public string Localise(string key)
    {
        if(_localisation.TryGetValue(key, out var value))
        {
            return value;
        }
        return $"%%%{key}$$$";
    }

    internal void SetLocal(string localKey)
    {
        LoadLocal(localKey);
    }
}
