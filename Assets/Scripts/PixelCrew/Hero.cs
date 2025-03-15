using PixelCrew.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    private Vector2 _direction;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
   
    private bool _isGraunded;
    private bool _allowDubleJump;
    private bool _isJumping;

    private Collider2D[] _interactionResult = new Collider2D[1];

    [SerializeField]private float _speed;
    [SerializeField] private float _jumpspeed;
    [SerializeField] private float _damageJumpspeed;
    [SerializeField] private float _interactionRadius;
    [SerializeField] private LayerMask _interactionLayer;
    [SerializeField] private LayerCheck _groundCheck;
    [SerializeField] private Spawn _footParticles;
    [SerializeField] private Spawn _jumpParticles;
    [SerializeField] private Spawn _fallParticles;
    [SerializeField] private ParticleSystem _hitParticles;

    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private Vector3 _groundCheckPositionDelta;
    [SerializeField] private LayerMask _groundLayer;


    public int Score { get; set; } = 0;

    private static readonly int IsGraund = Animator.StringToHash("Isground");
    private static readonly int Verticalvelocity = Animator.StringToHash("Verticalvelocity");
    private static readonly int Isrunning = Animator.StringToHash("Isrunning");
    private static readonly int Hit = Animator.StringToHash("hit");


    

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
      
    }
    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }
    private void Update()
    {
        _isGraunded = CheckWhenGroundedByCircleCollider();
        
    }
    private void FixedUpdate()
    {
        SpawnFallDust();
        var xVelocity = _direction.x * _speed;
        var yVelocity = CalculateVelocity();
        _rigidbody.velocity = new Vector2(xVelocity, yVelocity);
        
        _animator.SetBool(IsGraund, _isGraunded);
        _animator.SetFloat(Verticalvelocity, _rigidbody.velocity.y);
        _animator.SetBool(Isrunning, _direction.x != 0);
        UpdateSpriteDirection();
    }
    private float CalculateVelocity()
    {
        var yVelocity = _rigidbody.velocity.y;
   
        var IsJumpPressing = _direction.y > 0;
        if(_isGraunded)
        {
            _allowDubleJump = true;
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
        if(_rigidbody.velocity.y <= 0.1f && _rigidbody.velocity.y >= -0.1f && _isGraunded)
        {
            SpawnJumpDust();

        }
        if (_isGraunded)
        {
            yVelocity += _jumpspeed;
            
        }
        else if(_allowDubleJump)
        {
            yVelocity = _jumpspeed;
            SpawnJumpDust();
             _allowDubleJump = false;
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
    private bool CheckWhenGroundedByCircleCaste()
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
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpspeed);

        if (Score > 0)
        {
            SpawnCoins();
        }
        
     
    }
    private void SpawnCoins()
    {
        var numCoinsToDispose = Mathf.Min(Score, 5);
        Score -= numCoinsToDispose;

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
    private void OnDrawGizmos()
    {
        Gizmos.color = CheckWhenGroundedByCircleCaste() ? Color.green : Color.red;
      
        Gizmos.DrawSphere(transform.position + _groundCheckPositionDelta, _groundCheckRadius);
    }
    public void SpawnFootDust()
    {
        _footParticles.SpawnTarget();
    }
    public void SpawnJumpDust()
    {
        _jumpParticles.SpawnTarget();
    }
    public void SpawnFallDust()
    {
        if(_rigidbody.velocity.y < -13 && CheckWhenGroundedByCircleCaste())
        {
            _fallParticles.SpawnTarget();
        }
    }
    //void OnCollisionEnter2D(Collision2D col)
    //{
    //    if (col.transform.tag == "Platform") //передаем персонажу скорость движущихся платформ
    //        transform.parent = col.transform;
    //}
    //void OnCollisionExit2D(Collision2D col)
    //{
    //    if (col.transform.tag == "Platform") //убираем у персонажа скорость платформы
    //        transform.parent = null;
    //}


}
