using PixelCrew.Components;
using PixelCrew.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltraDamage : MonoBehaviour
{
    [SerializeField] private Cooldown _cooldown;
    [SerializeField] private Damage _damage;
    public void Use()
    {
        _damage.UltraDamage.Retain(this);
        _cooldown.Reset();
    }
    private void Update()
    {
        if (_cooldown.IsReady)
            _damage.UltraDamage.Release(this);
    }


}
