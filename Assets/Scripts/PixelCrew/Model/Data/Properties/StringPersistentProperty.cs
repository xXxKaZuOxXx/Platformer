﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringPersistentProperty : PrefsPersistentProperty<string>
{
    public StringPersistentProperty(string defaultValue, string key) : base(defaultValue, key)
    {
        Init();
    }

    protected override string Read(string defaultValue)
    {
        return PlayerPrefs.GetString(Key, defaultValue);
    }

    protected override void Write(string value)
    {
        PlayerPrefs.SetString(Key,value);
    }
}
