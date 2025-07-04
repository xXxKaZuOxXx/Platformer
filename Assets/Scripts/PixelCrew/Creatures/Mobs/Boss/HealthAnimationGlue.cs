using PixelCrew.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthAnimationGlue : MonoBehaviour
{
    [SerializeField] private Health _hp;
    [SerializeField] private Animator _animator;

    private static readonly int Health = Animator.StringToHash("health");
    private readonly CompositeDisposable _trash = new CompositeDisposable();
    private void Awake()
    {
        
        _trash.Retain( _hp._onChange.Subscribe(OnHealthChanged));
        OnHealthChanged(_hp.HealthValue);
    }

    private void OnHealthChanged(int health)
    {
        _animator.SetInteger(Health, health);

    }
    private void OnDestroy()
    {
        _trash.Dispose();
    }
}
