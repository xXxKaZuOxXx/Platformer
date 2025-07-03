using PixelCrew.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BlooadSplashOverlay : MonoBehaviour
{
    
    [SerializeField] private Transform _overlay;

    private Vector3 _overScale;
    private GameSession _session;
    private Animator _animator;

    private static readonly int Health = Animator.StringToHash("Health");
    private readonly CompositeDisposable _trash = new CompositeDisposable();
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _overScale = _overlay.localScale - Vector3.one;
        
        _session = FindObjectOfType<GameSession>();
        _trash.Retain(_session.Data.Hp.SubscribeAndInvoke(OnHpCnaged));
    }

    private void OnHpCnaged(int newValue, int _)
    {
        var maxHp = _session.StatsModel.GetValue(StatId.Hp);
        var hpNormalized = newValue/ maxHp;
        _animator.SetFloat(Health, hpNormalized);

        var overlayModifier = Mathf.Max(hpNormalized - 0.25f, 0f);
        _overlay.localScale = Vector3.one + _overScale * overlayModifier;
    }
    private void OnDestroy()
    {
        _trash.Dispose();
    }
}
