using PixelCrew;
using PixelCrew.Components;
using PixelCrew.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTrapAI : MonoBehaviour
{
    [SerializeField] private LayerCheck _vision;
    [Header("Melee")]
    [SerializeField] private CheckCircleOverlap _meleeAttack;
    [SerializeField] private LayerCheck _meleeCanAttack;
    [SerializeField] private Cooldown _meleeCooldown;
 
    [Header("Range")]
    [SerializeField] private Spawn _rangeAttack;
    [SerializeField] private Cooldown _rangeCooldown;

    private static readonly int Melee = Animator.StringToHash("Melee");
    private static readonly int Range = Animator.StringToHash("Range");

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if(_vision.IsTouchingLayer)
        {
            if (_meleeCanAttack.IsTouchingLayer)
            {
                if (_meleeCooldown.IsReady)
                {
                    MeleeAttack();
                }
                return;
            }
            if (_rangeCooldown.IsReady)
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
    private void MeleeAttack()
    {
        _meleeCooldown.Reset();
        _animator.SetTrigger(Melee);
    }
    public void OnMeleeAttack()
    {
        _meleeAttack.Check();
    }

}
