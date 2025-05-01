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
    private int CoinsCount => _session.Data.Inventory.Count("Coin");
    private int SwordCount => _session.Data.Inventory.Count("Sword");

    

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
        if (id == "Sword")
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
        if (!IsGrounded && _allowDoubleJump && !_isOnWall)
        {

            
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
       var poitions = _session.Data.Inventory.Count("Heal");
        if (poitions > 0)
        {
            var health = GetComponent<Health>();
            health.ApplyDamage(-5);
        }
        _session.Data.Inventory.Remove("Heal", 1);
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
        Particles.Spawn("Throw");
    }
    internal void Throw(bool triple)
    {
        if(_throwCooldown.IsReady && SwordCount > 1 && !triple)
        {
            Animator.SetTrigger(ThrowKey);
            _throwCooldown.Reset();
            _session.Data.Inventory.Remove("Sword", 1);
        }
        else if(SwordCount > 3 && triple)
        {
            
            StartCoroutine(ThreeThrows());
            
                //Animator.SetTrigger(ThrowKey);
                //_session.Data.Swords -= 1;
            
            _throwCooldown.Reset();

        }
        
    }
    private IEnumerator ThreeThrows()
    {
        for (int i = 0; i < 3; i++)
        {
            Animator.SetTrigger(ThrowKey);
            _session.Data.Inventory.Remove("Sword", 1);
            yield return new WaitForSeconds(0.2f);
        }
        yield break;
    }
}
