using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LocaliseImage : AbstractLocalizeComponent
{
    [SerializeField] private Image _icon;
    [SerializeField] private IconId[] _icons;

    protected override void Localise()
    {
        var iconData = _icons.FirstOrDefault(x => x.Id == LocalisationManager.I.LocalKey);
        if(iconData != null)
            _icon.sprite = iconData.Icon;

    }
}

[Serializable]
public class IconId
{
    public string Id;
    public Sprite Icon;
}
