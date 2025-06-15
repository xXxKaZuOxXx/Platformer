using PixelCrew.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class StatWidget : MonoBehaviour, IItemRenderer<StatDef>
{
    [SerializeField] private Image _icon;
    [SerializeField] private Text _name;
    [SerializeField] private Text _currentValue;
    [SerializeField] private Text _increaseValue;
    [SerializeField] private ProgressBar _progress;
    [SerializeField] private GameObject _selector;

    private GameSession _session;
    private StatDef _data;

    private void Start()
    {
        _session = FindObjectOfType<GameSession>();
        UpdateView();
    }

    private void UpdateView()
    {
        var statsModel = _session.StatsModel;

        _icon.sprite = _data.Icon;
        _name.text = LocalisationManager.I.Localise(_data.Name);
        float currentLevelValue = statsModel.GetValue(_data.Id);
        _currentValue.text = currentLevelValue.ToString(CultureInfo.InvariantCulture);

        var currentLevel = statsModel.GetCurrentLevel(_data.Id);
        var nextLevel = currentLevel + 1;
        float nextLevelValue = statsModel.GetValue(_data.Id, nextLevel);
        var increaseValue = nextLevelValue - currentLevelValue;
        _increaseValue.text = $"{increaseValue}";
        _increaseValue.gameObject.SetActive(increaseValue > 0);

        var maxLevels = DefsFacade.I.PlayerDef.GetStat(_data.Id).Levels.Length-1;
        _progress.SetProgress(currentLevel / (float)maxLevels);

        _selector.SetActive(_session.StatsModel.InterfaceSelectedStat.Value == _data.Id);
    }

    public void SetData(StatDef data, int index)
    {
        _data = data;
        if(_session != null)
            UpdateView();
    }

    public void OnSelect()
    {
        _session.StatsModel.InterfaceSelectedStat.Value = _data.Id;
    }
}
