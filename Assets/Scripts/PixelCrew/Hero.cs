using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    private Vector2 _direction;
    private Rigidbody2D _rigidbody;
    [SerializeField]private float _speed;
    [SerializeField] private float _jumpspeed;
  

    [SerializeField] private LayerCheck _groundCheck;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }
    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_direction.x * _speed, _rigidbody.velocity.y);
        var IsJumping = _direction.y > 0;
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
