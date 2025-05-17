using PixelCrew.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private ProgressBar _healthBar;

    private GameSession _session;
    private void Start()
    {
        _session = FindObjectOfType<GameSession>();
        _session.Data.Hp.OnChanged += OnHealthChanged;

        OnHealthChanged(_session.Data.Hp.Value,0);
    }

    private void OnHealthChanged(int newValue, int oldValue)
    {
        var maxHealth = DefsFacade.I.PlayerDef.MaxHealth;
        var value = (float)newValue / maxHealth;
        _healthBar.SetProgress(value);
    }
    public void OnSettings()
    {
        WindowUtils.CreateWindow("UI/InGameMenuWindow");
    }
    private void OnDestroy()
    {
        _session.Data.Hp.OnChanged -= OnHealthChanged;
    }
}
