using PixelCrew;
using PixelCrew.Components;
using PixelCrew.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemsAI : MonoBehaviour
{
    [SerializeField] private LayerCheck _vision;
    [SerializeField] private Spawn _rangeAttack;
    [SerializeField] private Cooldown _rangeCooldown;
    [SerializeField] private Cooldown _timeBeforeAttack;

    private Animator _animator;
    private Health _health;
    private float _currentHealth;
    private bool _canAttack = false;

    private static readonly int Range = Animator.StringToHash("Attack");
    private static readonly int Hit = Animator.StringToHash("Hit");

    private void Awake()
    {
        _health = GetComponent<Health>();
        _currentHealth = _health.HealthValue;
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        
        if(_currentHealth >  _health.HealthValue)
        {
            _animator.SetTrigger(Hit);
            _currentHealth = _health.HealthValue;
        }
        
        if (_vision.IsTouchingLayer)
        {
            if (!_canAttack)
            {
                _timeBeforeAttack.Reset();
                _canAttack = true;
            }
            if (_rangeCooldown.IsReady && _timeBeforeAttack.IsReady)
            {
                RangeAttack();
            }
        }
    }
    

    private void RangeAttack()
    {
        _rangeCooldown.Reset();
        _animator.SetTrigger(Range);
    }

    public void OnRangeAttack()
    {

        _rangeAttack.SpawnTarget();
    }

    
}
