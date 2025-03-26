using PixelCrew;
using PixelCrew.Components;
using PixelCrew.Model;
using PixelCrew.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class Hero : MonoBehaviour
{
    private Vector2 _direction;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
   
    private bool _isGrounded;
    private bool _allowDoubleJump;
    private bool _isJumping;
    //private bool _isArmed;

    private Collider2D[] _interactionResult = new Collider2D[1];

    [SerializeField]private float _speed;
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private float _damageJumpSpeed;
    [SerializeField] private float _interactionRadius;
    [SerializeField] private LayerMask _interactionLayer;
    [SerializeField] private LayerCheck _groundCheck;

    [SerializeField] private Spawn _attackParticles;
    [SerializeField] private Spawn _footParticles;
    [SerializeField] private Spawn _jumpParticles;
    [SerializeField] private Spawn _fallParticles;
    [SerializeField] private ParticleSystem _hitParticles;

    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private Vector3 _groundCheckPositionDelta;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private CheckCircleOverlap _attackRange;
    [SerializeField] private int _damage;

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
   

    private GameSession _session;

    [SerializeField] private AnimatorController _armed;
    [SerializeField] private AnimatorController _unarmed;

    private static readonly int IsGround = Animator.StringToHash("Isground");
    private static readonly int Verticalvelocity = Animator.StringToHash("Verticalvelocity");
    private static readonly int Isrunning = Animator.StringToHash("Isrunning");
    private static readonly int Hit = Animator.StringToHash("hit");
    private static readonly int attack = Animator.StringToHash("attack");

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
      
    }
    private void Start()
    {
        _session = FindObjectOfType<GameSession>();
        var health = GetComponent<Health>();
        health.SetHealth(_session.Data.Hp);
        UpdateHeroWeapon();

    }
    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }
    public void OnHealthChanged(int currentHealth)
    {
        _session.Data.Hp = currentHealth;
    }
    private void Update()
    {
        _isGrounded = CheckWhenGroundedByCircleCollider();
        
    }
    private void FixedUpdate()
    {
        SpawnFallDust();
        var xVelocity = _direction.x * _speed;
        var yVelocity = CalculateVelocity();
        _rigidbody.velocity = new Vector2(xVelocity, yVelocity);
        
        _animator.SetBool(IsGround, _isGrounded);
        _animator.SetFloat(Verticalvelocity, _rigidbody.velocity.y);
        _animator.SetBool(Isrunning, _direction.x != 0);
        UpdateSpriteDirection();
    }
    private float CalculateVelocity()
    {
        var yVelocity = _rigidbody.velocity.y;
   
        var IsJumpPressing = _direction.y > 0;
        if(_isGrounded)
        {
            _allowDoubleJump = true;
            _isJumping = false;
        }
        if (IsJumpPressing)
        {
            _isJumping = true;
            
            yVelocity = CalculateJumpVelocity(yVelocity);
            
        }
        else if (_rigidbody.velocity.y > 0 && _isJumping)
        {
            yVelocity *= 0.5f;
        }
        return yVelocity;
    }
    private float CalculateJumpVelocity(float yVelocity)
    {
        var isFallyng = _rigidbody.velocity.y <= 0.001f;
        if (!isFallyng)
            return yVelocity;
        if(_rigidbody.velocity.y <= 0.1f && _rigidbody.velocity.y >= -0.1f && _isGrounded)
        {
            SpawnJumpDust();

        }
        if (_isGrounded)
        {
            yVelocity += _jumpSpeed;
            
        }
        else if(_allowDoubleJump)
        {
            yVelocity = _jumpSpeed;
            SpawnJumpDust();
             _allowDoubleJump = false;
        }
        return yVelocity;
    }
    private void UpdateSpriteDirection()
    {
        if (_direction.x > 0)
        {
            transform.localScale = Vector3.one;
           
        }
        else if (_direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            
        }
    }
    private bool CheckWhenGroundedByCircleCollider()
    {
       
        return _groundCheck.IsTouchingLayer;
        
    }
    private bool CheckWhenGroundedByCircleCast()
    {
        var hit = Physics2D.CircleCast(transform.position + _groundCheckPositionDelta, _groundCheckRadius, Vector2.down, 0, _groundLayer);
        return hit.collider != null;
    }
   
    public void SaySomething()
    {
        Debug.Log("Some");
    }
    public void TakeDamage()
    {
        _isJumping = false;
        _animator.SetTrigger(Hit);
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpSpeed);

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
        var size = Physics2D.OverlapCircleNonAlloc(transform.position, _interactionRadius, _interactionResult, _interactionLayer);

        for(int i = 0; i < size; i++)
        {
            var interactable = _interactionResult[i].GetComponent<Interactive>();
            if(interactable != null)
            {
                interactable.Interact();
            }
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = CheckWhenGroundedByCircleCast() ? HandlesUtils.TransparentGreen : HandlesUtils.TransparentRed;
        Handles.DrawSolidDisc(transform.position + _groundCheckPositionDelta, Vector3.forward, _groundCheckRadius);
    }
#endif 
    public void SpawnFootDust()
    {
        _footParticles.SpawnTarget();
    }
    public void SpawnAttack1Dust()
    {
        _attackParticles.SpawnTarget();
    }
    public void SpawnJumpDust()
    {
        _jumpParticles.SpawnTarget();
    }
    public void SpawnFallDust()
    {
        if(_rigidbody.velocity.y < -13 && CheckWhenGroundedByCircleCast())
        {
            _fallParticles.SpawnTarget();
        }
    }

    public void Attack()
    {
        if(!_session.Data.IsArmed) return;

        _animator.SetTrigger(attack);
        SpawnAttack1Dust();


    }

    private void OnAttack()
    {
        var gos = _attackRange.GetObjectsInRange();
        foreach (var item in gos)
        {
            var hp = item.GetComponent<Health>();
            if (hp != null && item.CompareTag("Enemy") && hp > 0)
            {
                hp.ApplyDamage(_damage);
            }
        }
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
            _animator.runtimeAnimatorController = _armed;
        }
        else
        {
            _animator.runtimeAnimatorController = _unarmed;
        }
    }
}
