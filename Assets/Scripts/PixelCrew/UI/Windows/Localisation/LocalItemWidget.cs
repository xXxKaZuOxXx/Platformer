using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LocalItemWidget : MonoBehaviour, IItemRenderer<LocalInfo>
{
    [SerializeField] private Text _text;
    [SerializeField] private GameObject _selector;
    [SerializeField] private SelectLocal _onSelected;

    private LocalInfo _data;

    private void Start()
    {
        LocalisationManager.I.OnLocalChanged += UpdateSelection;
    }
    public void SetData(LocalInfo localInfo, int index)
    {
        _data = localInfo;
        UpdateSelection();
        _text.text = localInfo.LocalId.ToUpper();
    }
    private void UpdateSelection()
    {
        var isSelected = LocalisationManager.I.LocalKey == _data.LocalId;
        _selector.SetActive(isSelected);
    }

    private void OnDestroy()
    {
        LocalisationManager.I.OnLocalChanged -= UpdateSelection;
    }

    public void OnSelected()
    {
        _onSelected?.Invoke(_data.LocalId);
    }
}

[Serializable]
public class SelectLocal : UnityEvent<string>
{

}
public class LocalInfo
{
    public string LocalId;
}
