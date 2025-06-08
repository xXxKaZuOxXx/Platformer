using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct DialogData
{
    [SerializeField] public Sentense[] _sentences;
    [SerializeField] private DialogType _type;
    [SerializeField] public string[] _localisationKeys;

    public Sentense [] Sentences => _sentences;
    public DialogType Type => _type;

}
[Serializable]
public struct Sentense
{
    [SerializeField] public string _value;
    [SerializeField] private Sprite _icon;
    [SerializeField] private Side _side;
    public string Value => _value;
    public Sprite Icon => _icon;
    public Side Side => _side;
}
public enum Side
{
    Left,
    Right
}
public enum DialogType
{
    Simple,
    Personalized
}
