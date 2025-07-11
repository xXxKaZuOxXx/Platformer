﻿using PixelCrew.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace PixelCrew.Creatures
{
    public class Creature : MonoBehaviour
    {
        [Header("Params")]
        [SerializeField] private bool _invertScale;
        [SerializeField] protected float _speed;
        [SerializeField] protected float JumpSpeed;
        [SerializeField] private float _damageVelocity;
      

        [Header("Checkers")]
        [SerializeField] protected LayerMask GroundLayer;
        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] protected CheckCircleOverlap _attackRange;
        [SerializeField] protected SpawnListComponent Particles;

        protected Vector2 Direction;
        protected Rigidbody2D Rigidbody;
        protected Animator Animator;
        protected PlaySounds Sounds;
        protected bool IsGrounded;
        private bool _isJumping;

        private static readonly int IsGround = Animator.StringToHash("Isground");
        private static readonly int Verticalvelocity = Animator.StringToHash("Verticalvelocity");
        private static readonly int Isrunning = Animator.StringToHash("Isrunning");
        private static readonly int Hit = Animator.StringToHash("hit");
        private static readonly int attack = Animator.StringToHash("attack");

        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
            Sounds = GetComponent<PlaySounds>();
           
        }
        public void SetDirection(Vector2 direction)
        {
            Direction = direction;
        }
        protected virtual void Update()
        {
            IsGrounded = _groundCheck.IsTouchingLayer;
        }
        protected virtual void FixedUpdate()
        {
            //SpawnFallDust();
            var xVelocity = CalculateXVelocity();
            var yVelocity = CalculateVelocity();
            Rigidbody.velocity = new Vector2(xVelocity, yVelocity);

            Animator.SetBool(IsGround, IsGrounded);
            Animator.SetFloat(Verticalvelocity, Rigidbody.velocity.y);
            Animator.SetBool(Isrunning, Direction.x != 0);
            UpdateSpriteDirection(Direction);
        }
        protected virtual float CalculateXVelocity()
        {

            return Direction.x * CalculateSpeed();
        }
        protected virtual float CalculateSpeed()
        {
            return _speed;
        }
        protected virtual float CalculateVelocity()
        {
            var yVelocity = Rigidbody.velocity.y;

            var IsJumpPressing = Direction.y > 0;
            if (IsGrounded)
            {

                _isJumping = false;
            }
            if (IsJumpPressing)
            {
                _isJumping = true;
                var isFallyng = Rigidbody.velocity.y <= 0.001f;

                yVelocity = isFallyng ? CalculateJumpVelocity(yVelocity) : yVelocity;

            }
            else if (Rigidbody.velocity.y > 0 && _isJumping)
            {
                yVelocity *= 0.5f;
            }
            return yVelocity;
        }
        protected virtual float CalculateJumpVelocity(float yVelocity)
        {

            if (IsGrounded)
            {
                yVelocity += JumpSpeed;
                DoJumpVfx();
            }

            return yVelocity;
        }

        protected void DoJumpVfx()
        {
            Profiler.BeginSample("JumpVFXSample");
            Particles.Spawn("Jump");
            Profiler.EndSample();
            Sounds.Play("Jump");
        }

        public void UpdateSpriteDirection(Vector2 direction)
        {
            var multiplier = _invertScale ? -1 : 1;
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(multiplier, 1, 1);

            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1 * multiplier, 1, 1);

            }
        }
        public virtual void TakeDamage()
        {
            _isJumping = false;
            Animator.SetTrigger(Hit);
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, _damageVelocity);
        }
        public virtual void Attack()
        {
            Animator.SetTrigger(attack);
            Sounds.Play("Melee");
        }
        public virtual void OnAttack()
        {
            Particles.Spawn("Attack1");
            

            _attackRange.Check();
            
        }

    }
}

