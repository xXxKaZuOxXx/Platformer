using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    private Vector2 _direction;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private SpriteRenderer _sprite;

    [SerializeField]private float _speed;
    [SerializeField] private float _jumpspeed;
   
    public int Score { get; set; } = 0;

    private static readonly int IsGraund = Animator.StringToHash("Isground");
    private static readonly int Verticalvelocity = Animator.StringToHash("Verticalvelocity");
    private static readonly int Isrunning = Animator.StringToHash("Isrunning");


    [SerializeField] private LayerCheck _groundCheck;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
    }
    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }
    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_direction.x * _speed, _rigidbody.velocity.y);
        var IsJumping = _direction.y > 0;
        var isGraunded = IsGrounded();
        if(IsJumping )
        {
            if( IsGrounded())
            {
                _rigidbody.AddForce(Vector2.up * _jumpspeed, ForceMode2D.Impulse);
            }
        }
        else if (_rigidbody.velocity.y > 0)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * 0.5f);
        }
        _animator.SetBool(IsGraund, isGraunded);
        _animator.SetFloat(Verticalvelocity, _rigidbody.velocity.y);
        _animator.SetBool(Isrunning, _direction.x != 0);
        UpdateSpriteDirection();
    }
    private void UpdateSpriteDirection()
    {
        if (_direction.x > 0)
        {
            _sprite.flipX = false;
        }
        else if (_direction.x < 0)
        {
            _sprite.flipX = true;
        }
    }
    private bool IsGrounded()
    {
        return _groundCheck.IsTouchingLayer;
        
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = IsGrounded() ? Color.green : Color.red;
    //    Gizmos.DrawSphere(transform.position, 0.3f);
    //}
    public void SaySomething()
    {
        Debug.Log("Some");
    }
}
