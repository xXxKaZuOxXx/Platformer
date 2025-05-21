using PixelCrew.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBarWidget : MonoBehaviour
{
    [SerializeField] private ProgressBar _lifeBar;
    [SerializeField] private Health _hp;

    private readonly CompositeDisposable _trash = new CompositeDisposable();
    private int _maxHp;
    private void Start()
    {
        if(_hp ==  null)
        {
            _hp = GetComponentInParent<Health>();
        }
        _maxHp = _hp.HealthValue;

        _trash.Retain(_hp._onDie.Subscribe(OnDie));
        _trash.Retain(_hp._onChange.Subscribe(OnHpChanged));
    }

    private void OnDie()
    {
        Destroy(gameObject);
    }

    private void OnHpChanged(int hp)
    {
        var progress = (float) hp/_maxHp;
        _lifeBar.SetProgress(progress);
    }
    private void OnDestroy()
    {
        _trash.Dispose();
    }
}
