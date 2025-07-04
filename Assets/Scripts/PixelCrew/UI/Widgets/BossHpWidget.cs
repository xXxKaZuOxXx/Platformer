using PixelCrew.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHpWidget : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private ProgressBar _hpBar;
    [SerializeField] private CanvasGroup _canvas;
    
    private float _maxHealth;
    private readonly CompositeDisposable _trash = new CompositeDisposable();
    private void Start()
    {
        _maxHealth = _health.HealthValue;
        _trash.Retain(_health._onChange.Subscribe(OnHpChanged));
        _trash.Retain(_health._onDie.Subscribe(HideUI));
    }

    [ContextMenu("Show")]
    public void ShowUI()
    {
        this.LerpAnimated(0,1,1, SetAlpha);
    }
    private void SetAlpha(float alpha)
    {
        _canvas.alpha = alpha;
    }
    [ContextMenu("Hide")]
    public void HideUI()
    {
        this.LerpAnimated(1, 0, 1, SetAlpha);
    }

    private void OnHpChanged(int health)
    {
        _hpBar.SetProgress(health/ _maxHealth);
    }
    private void OnDestroy()
    {
        _trash.Dispose();
    }
}
