using PixelCrew;
using PixelCrew.Components;
using PixelCrew.Creatures;
using PixelCrew.Model;
using PixelCrew.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class Hero : Creature, IcanAddInInventory
{
   

   
    [SerializeField] private CheckCircleOverlap _interactionCheck;
    [SerializeField] private float _slamDownVelocity;
    [SerializeField] private LayerCheck _wallCheck;

    [SerializeField] private Cooldown _throwCooldown;
    [SerializeField] private AnimatorController _armed;
    [SerializeField] private AnimatorController _unarmed;
    [SerializeField] private Spawn _throwSpawner;
    [SerializeField] private Shield _shield;

    private static readonly int ThrowKey = Animator.StringToHash("throw");
    private static readonly int IsOnWall = Animator.StringToHash("IsOnWall");
    [SerializeField] private ParticleSystem _hitParticles;

    private GameSession _session;
    private float _defaultGravityScale;

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
        if(_session.PerksModel.IsShieldSupported)
        {
            _shield.Use();
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
        var health = GetComponent<Health>();
        _session.Data.Inventory.OnChanged += OnInventoryChanged;
        health.SetHealth(_session.Data.Hp.Value);
        UpdateHeroWeapon();

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
        _throwSpawner.SpawnTarget();
        _session.Data.Inventory.Remove(throwableId, 1);
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

        return base.CalculateSpeed() + _additionalSpeed; 
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
}
