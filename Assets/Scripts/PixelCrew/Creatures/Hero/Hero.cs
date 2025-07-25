﻿using PixelCrew;
using PixelCrew.Components;
using PixelCrew.Creatures;
using PixelCrew.Model;
using PixelCrew.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
//using UnityEditor.Animations;
using UnityEngine;

public class Hero : Creature, IcanAddInInventory
{
   

   
    [SerializeField] private CheckCircleOverlap _interactionCheck;
    [SerializeField] private float _slamDownVelocity;
    [SerializeField] private LayerCheck _wallCheck;

    [SerializeField] private Cooldown _throwCooldown;
    [SerializeField] private RuntimeAnimatorController _armed;
    [SerializeField] private RuntimeAnimatorController _unarmed;
    [SerializeField] private Spawn _throwSpawner;
    [SerializeField] private Shield _shield;
    [SerializeField] private HeroFlashLight _flashlight;

    private static readonly int ThrowKey = Animator.StringToHash("throw");
    private static readonly int IsOnWall = Animator.StringToHash("IsOnWall");
    [SerializeField] private ParticleSystem _hitParticles;

    private GameSession _session;
    private Health _health;
    private float _defaultGravityScale;
    public float dashPower = 1;
    private CameraShakeEffect _cameraShake;
    private Damage _damage;
    private UltraDamage _ultraDamage;


    private bool _allowDoubleJump;
    private bool _isOnWall;
    
    public void AddInInventory(string id, int value)
    {
        _session.Data.Inventory.Add(id, value);
    }

    private const string SwordId = "Sword";
    private int CoinsCount => _session.Data.Inventory.Count("Coin");
    private int SwordCount => _session.Data.Inventory.Count(SwordId);

    private string SelectedId => _session.QuickInventory.SelectedItem.Id;
    private bool _canSuperThrow = false;
    private bool CanThrow
    {
        get
        {
            var def = DefsFacade.I.Items.Get(SelectedId);
            if (def.HasTag(ItemTag.Throwable) && _session.Data.Inventory.Count(SelectedId) > 3)
            {
                _canSuperThrow = true;
            }
            
            if (SelectedId == SwordId)
                return SwordCount > 1; 
            if(_session.Data.Inventory.Count(SelectedId) > 0)
                return def.HasTag(ItemTag.Throwable);
            return false;
        }
    }
    public void UsePerk()
    {
        if (_session.PerksModel.IsShieldSupported)
        {
            _shield.Use();
            _session.PerksModel.Cooldown.Reset();
        }
        else if (_session.PerksModel.IsUltraPowerSupported)
        {
            _ultraDamage.Use();
            // дописать зависимости
            _session.PerksModel.Cooldown.Reset();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        _defaultGravityScale = Rigidbody.gravityScale;
    }
    private void OnDestroy()
    {
        _session.Data.Inventory.OnChanged -= OnInventoryChanged;
    }

    private void Start()
    {
       
        _session = FindObjectOfType<GameSession>();
        _health = GetComponent<Health>();
        _ultraDamage = GetComponent<UltraDamage>();
        _damage = GetComponentInChildren<Damage>();
        _session.Data.Inventory.OnChanged += OnInventoryChanged;
        _session.StatsModel.OnUpgraded += OnHeroUpgraded;
        _health.SetHealth(_session.Data.Hp.Value);
        _cameraShake = FindObjectOfType<CameraShakeEffect>();
        UpdateHeroWeapon();

    }
    
    private void OnHeroUpgraded(StatId state)
    {
        switch (state)
        {
            case StatId.Hp:
                var health = (int)_session.StatsModel.GetValue(state);
                _session.Data.Hp.Value = health;
                _health.SetHealth(health);
                break;
        }
    }

    private void OnInventoryChanged(string id, int value)
    {
        if (id == SwordId)
            UpdateHeroWeapon();
    }


    public void OnHealthChanged(int currentHealth)
    {
        _session.Data.Hp.Value = currentHealth;
    }
    protected override void Update()
    {
        base.Update();
       
        var moveToSameDirection = Direction.x * transform.lossyScale.x > 0;
        if (_wallCheck.IsTouchingLayer && moveToSameDirection)
        {
            _isOnWall = true;
            Rigidbody.gravityScale = 0;
        }
        else
        {
            _isOnWall = false;
            Rigidbody.gravityScale = _defaultGravityScale;
        }

        Animator.SetBool(IsOnWall, _isOnWall);
    }
   
    
    protected override float CalculateVelocity()
    {
        
        var IsJumpPressing = Direction.y > 0;

        if (IsGrounded || _isOnWall)
        {
            _allowDoubleJump = true;
        }
        if ((IsJumpPressing && _isOnWall) || (!IsJumpPressing && _isOnWall))
        {
            return 0f;
        }

        return base.CalculateVelocity();
    }
    protected override float CalculateJumpVelocity(float yVelocity)
    {
        if (!IsGrounded && _allowDoubleJump && _session.PerksModel.IsDubleJumpSupported && !_isOnWall)
        {
            _session.PerksModel.Cooldown.Reset();

            _allowDoubleJump = false;
            DoJumpVfx();
            return JumpSpeed;
        }
        return base.CalculateJumpVelocity(yVelocity);
    }

    public void SaySomething()
    {
        Debug.Log("Some");
    }
    
    public override void TakeDamage()
    {
        base.TakeDamage();
        if (_cameraShake != null)
        {
            _cameraShake.Shake();
        }
       
        if (CoinsCount > 0)
        {
            SpawnCoins();
        }
    }
    private void SpawnCoins()
    {
        var numCoinsToDispose = Mathf.Min(CoinsCount, 5);
        _session.Data.Inventory.Remove("Coin", numCoinsToDispose);
       
        var burst = _hitParticles.emission.GetBurst(0);
        burst.count = numCoinsToDispose;
        _hitParticles.emission.SetBurst(0, burst);
        _hitParticles.gameObject.SetActive(true);
        _hitParticles.Play();
    }
    public void Interact()
    {
        _interactionCheck.Check();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.IsInLayer(GroundLayer))
        {
            var contact = collision.contacts[0];
            if(contact.relativeVelocity.y >= _slamDownVelocity)
            {
                Particles.Spawn("SlamDown");
            }
        }
    }
    

    public override void Attack()
    {
        
        if (SwordCount <= 0) return;

        base.Attack();

    }
    public override void OnAttack()
    {
        Particles.Spawn("Attack1");
        var damage = _attackRange.GetComponent<Damage>();
        var damageValue = damage.DamageValue;
        //var damageValue = (int)_session.StatsModel.GetValue(StatId.RangeDamage);
        damageValue = ModifyDamageCrit(damageValue);
        damage.SetDelta(damageValue);
        _attackRange.Check();
    }

    public void DoHeal()
    {
        //var healableId = _session.QuickInventory.SelectedItem.Id;
        //var healableDef = DefsFacade.I.HealItems.Get(healableId);
        //var def = DefsFacade.I.Items.Get(SelectedId);
        //if (def.HasTag(ItemTag.Healable))
        //{
        //    healableDef.Prefab.GetComponent<Damage>().ApplyDamage(this.gameObject);
        //    _session.Data.Inventory.Remove(healableId, 1);
        //}

    
    }
  
    private void UpdateHeroWeapon()
    {
        
        if (SwordCount > 0)
        {
            Animator.runtimeAnimatorController = _armed;
        }
        else
        {
            Animator.runtimeAnimatorController = _unarmed;
        }
    }
    public void OnDoThrow()
    {
        Sounds.Play("Range");
        
        
        var throwableId = _session.QuickInventory.SelectedItem.Id;
        var throwableDef = DefsFacade.I.Throwable.Get(throwableId);
        _throwSpawner.SetPrefab(throwableDef.Projectile);
        var instance = _throwSpawner.SpawnInstance();
        ApplyRangeDamageStat(instance);


        _session.Data.Inventory.Remove(throwableId, 1);
    }

    private void ApplyRangeDamageStat(GameObject projectile)
    {
        var damage = projectile.GetComponent<Damage>();
        var damageValue = (int)_session.StatsModel.GetValue(StatId.RangeDamage);
        damageValue = ModifyDamageCrit(damageValue);
        damage.SetDelta(damageValue);
    }
    private int ModifyDamageCrit(int damage)
    {
        var critChanse = _session.StatsModel.GetValue(StatId.CriticalDamage);
        System.Random random = new System.Random();
        if (random.Next(100) <= critChanse)
        {
            return damage * 2;
        }
        else
        {
            return damage;
        }
    }
    public void UseInventory(bool triple)
    {
        
        var isThrowable = IsSelectedItem(ItemTag.Throwable);
        if (isThrowable)
        {

            PerformThrowing(triple);
        }
        else if(IsSelectedItem(ItemTag.Potion))
        {
            UsePoition();
        }
        
        
    }

    private void UsePoition()
    {
        var potion = DefsFacade.I.Poitions.Get(SelectedId);
        switch(potion.Effect)
        {
            case Effect.AddHp:
                _session.Data.Hp.Value += (int)potion.Value;
                _health.ApplyDamage(-(int)potion.Value);
                break;
            case Effect.SpeedUp:
                _speedUpCooldown.Value = _speedUpCooldown.RemainingTime + potion.Time;
                _additionalSpeed = Mathf.Max(potion.Value, _additionalSpeed);
                _speedUpCooldown.Reset();
                break;
            default: throw new ArgumentOutOfRangeException();
        }
       
        _session.Data.Inventory.Remove(potion.Id, 1);
    }
    private readonly Cooldown _speedUpCooldown = new Cooldown();
    private float _additionalSpeed;
    protected override float CalculateSpeed()
    {
        if (_speedUpCooldown.IsReady)
            _additionalSpeed = 0f;

        float defaultSpeed = _session.StatsModel.GetValue(StatId.Speed);
        return defaultSpeed + _additionalSpeed; 
    }
    private bool IsSelectedItem(ItemTag tag)
    {
        return _session.QuickInventory.SelectedDef.HasTag(tag);
    }
    public void PerformThrowing(bool triple)
    {
        if (_throwCooldown.IsReady && CanThrow && !triple)
        {
            Animator.SetTrigger(ThrowKey);
            _throwCooldown.Reset();

        }
        else if (CanThrow && triple && _canSuperThrow && _session.PerksModel.IsSupperThrowSupported)
        {
            _session.PerksModel.Cooldown.Reset();
            StartCoroutine(ThreeThrows());

            //Animator.SetTrigger(ThrowKey);
            //_session.Data.Swords -= 1;
            _canSuperThrow = false;
            _throwCooldown.Reset();

        }
    }
    private IEnumerator ThreeThrows()
    {
        for (int i = 0; i < 3; i++)
        {
            Animator.SetTrigger(ThrowKey);
            
            yield return new WaitForSeconds(0.2f);
        }
        yield break;
    }

    public void NextItem()
    {
        _session.QuickInventory.SetNextItem();
    }
    protected override float CalculateXVelocity()
    {
        Dash(DoDash);

        return Direction.x * CalculateSpeed()* dashPower;
    }

    private void Dash(bool doDash)
    {
        if (!DoDash)
        {
            dashPower = 1;
        }
        if (DoDash && _session.PerksModel.IsDashSupported)
        {
            dashPower = 35;
            _session.PerksModel.Cooldown.Reset();
            DoDash = false;
        }
    }
    public void OnDash()
    {
        DoDash = true;
    }
    public bool DoDash {  get;  set; }
    
    public void ToggleFlashlight()
    {
        var isActtive = _flashlight.gameObject.activeSelf;
        _flashlight.gameObject.SetActive(!isActtive);

    }
    
}
