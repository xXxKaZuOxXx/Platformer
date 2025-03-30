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

public class Hero : Creature
{
   

    [SerializeField] private float _interactionRadius;
    [SerializeField] private CheckCircleOverlap _interactionCheck;
    [SerializeField] private float _slamDownVelocity;

    [SerializeField] private ParticleSystem _hitParticles;

    [SerializeField] private AnimatorController _armed;
    [SerializeField] private AnimatorController _unarmed;

   

    private GameSession _session;

    private bool _allowDoubleJump;
   
    public int Score
    {
        get
        {
            return _session.Data.Coins;
        }
        set
        {
            _session.Data.Coins = value;
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }
   
    private void Start()
    {
        _session = FindObjectOfType<GameSession>();
        var health = GetComponent<Health>();
        health.SetHealth(_session.Data.Hp);
        UpdateHeroWeapon();

    }
    
    public void OnHealthChanged(int currentHealth)
    {
        _session.Data.Hp = currentHealth;
    }
    protected override void Update()
    {
        base.Update();
    }
   

    protected override float CalculateVelocity()
    {
        var yVelocity = Rigidbody.velocity.y;
        var IsJumpPressing = Direction.y > 0;

        if (IsGrounded)
        {
            _allowDoubleJump = true;
        }
        return base.CalculateVelocity();
    }
    protected override float CalculateJumpVelocity(float yVelocity)
    {
        if (!IsGrounded && _allowDoubleJump)
        {

            Particles.Spawn("Jump");
            _allowDoubleJump = false;
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
        if (_session.Data.Coins > 0)
        {
            SpawnCoins();
        }
    }
    private void SpawnCoins()
    {
        var numCoinsToDispose = Mathf.Min(_session.Data.Coins, 5);
        _session.Data.Coins -= numCoinsToDispose;

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
        if(!_session.Data.IsArmed) return;

        base.Attack();


    }

   

    internal void ArmedHero()
    {
        _session.Data.IsArmed = true;
        UpdateHeroWeapon();
        //_animator.runtimeAnimatorController = _armed;
    }
    private void UpdateHeroWeapon()
    {
        if(_session.Data.IsArmed)
        {
            Animator.runtimeAnimatorController = _armed;
        }
        else
        {
            Animator.runtimeAnimatorController = _unarmed;
        }
    }
}
