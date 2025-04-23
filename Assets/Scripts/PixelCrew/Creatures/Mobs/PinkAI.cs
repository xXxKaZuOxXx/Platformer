using PixelCrew.Components;
using PixelCrew.Creatures;
using PixelCrew.Model;
using PixelCrew;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkAI : MonoBehaviour
{
    [SerializeField] private LayerCheck _vision;
    [SerializeField] private LayerCheck _canAttack;
    [SerializeField] private LayerCheck _attackVision;
    [SerializeField] private CheckCircleOverlap _attackRange;

    [SerializeField] private float _alarmDelay = 0.5f;
    [SerializeField] private float _attackCooldown = 1f;
    [SerializeField] private float _missCooldown = 0.5f;
    private Coroutine _current;
    private GameObject _target;

    private static readonly int IsDeadKey = Animator.StringToHash("IsDead");
    private static readonly int IsAttackKey = Animator.StringToHash("attack");
    private static readonly int IsGroundKey = Animator.StringToHash("ground");

    private SpawnListComponent _particles;
    private Creature _creature;
    private Animator _animator;
    private bool _isDead;
    private bool _attacking = false;
    private float _treshold = 0.7f;
    private Patrol _patrol;
    private Rigidbody2D _rigidbody2D;
    private float _stunTime = 0.5f;
    private bool _canDoCour = true;
    // нужно сделать так чтобы он когда видел героя крутился в его сторону и енсли задаенет бил 
    //если не задел то с прошлой позиции героя он прокручивается на 2-4 единицы вперед
    //после встает в ожидание и через секунду отпрыгивает от героя потом повторяется все
    private void Awake()
    {
        _particles = GetComponent<SpawnListComponent>();
        _creature = GetComponent<Creature>();
        _animator = GetComponent<Animator>();
        _patrol = GetComponent<Patrol>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartState(_patrol.DoPatrol());
    }

    public void OnHeroInVision(GameObject go)
    {
        if (_isDead) return;
        if (_attacking) return;
        if(!_canDoCour) return;
        _target = go;
        LookAtHero();
        StartState(Attack());
        Debug.Log("SSS");
    }
    private IEnumerator AgroToHero()
    {
        LookAtHero();

        yield return null;
        StartState(Attack());
    }
    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(_alarmDelay);

        var pointToAttack = _target.transform.position;
        var direction = _target.transform.position;

        _attacking = true;
        //_creature.SetDirection(Vector2.zero);
        _animator.SetBool(IsAttackKey, true);
       

        if (pointToAttack.x - transform.position.x < 0)
        {
            pointToAttack -= new Vector3(0.5f, 0, 0);
            pointToAttack.y = 0;
        }
        else
        {
            pointToAttack += new Vector3(0.5f, 0, 0);
            pointToAttack.y = 0;
        }

        var enemyDir = transform.position;
        enemyDir.y = 0;
        while (_attacking && ((pointToAttack - enemyDir).magnitude > _treshold))
        {

            pointToAttack.y = 0;
            var dir = (pointToAttack - transform.position);
            dir.y = 0;


            _creature.SetDirection(dir.normalized * 2);


            enemyDir = transform.position;
            enemyDir.y = 0;
            if ((pointToAttack - enemyDir).magnitude < _treshold)
            {
                _attacking = false;
            }

            yield return null;

        }

        //Debug.Log((pointToAttack - enemyDir).magnitude);


        //_creature.SetDirection(Vector2.zero);
        _creature.SetDirection(Vector2.zero);
        _animator.SetBool(IsAttackKey, false);
        //StopCoroutine(_current);
        
        StartState(JumpAway());




    }
    public void Strike()
    {
        _attackRange.Check();
        _particles.Spawn("Attack1");
    }
    private IEnumerator JumpAway()
    {
        _canDoCour = false;
        _creature.SetDirection(Vector2.zero);
        yield return new WaitForSeconds(_stunTime);
        

        var directionToJump = (_target.transform.position - transform.position);
        directionToJump.y = 0;
        var dir = directionToJump.normalized;
        var vel = new Vector2(-dir.x, 1);
        _creature.SetDirection(vel);
        //var foce = new Vector2(dir.x * 50, 80 ); // доделать распрыжку
        //_rigidbody2D.AddForce(foce);
        yield return null;
        
        while (!(_animator.GetCurrentAnimatorStateInfo(0).IsName("ground")))
        {
            
            yield return null;
        }
        _creature.SetDirection(Vector2.zero);
        //yield return new WaitForSeconds(_attackCooldown);
        _canDoCour = true;
        
        
        
        if(!_attackVision.IsTouchingLayer)
        {
            StartState(_patrol.DoPatrol());
           
        }
        else
        {
            StartState(Attack());
        }
       
          
        



    }
    private void LookAtHero()
    {
        var direction = GetDirectionToTarget();
        _creature.UpdateSpriteDirection(direction);
    }

    private Vector2 GetDirectionToTarget()
    {
        var direction = _target.transform.position - transform.position;
        direction.y = 0;
        return direction.normalized;
    }

    private void StartState(IEnumerator coroutine)
    {
        _creature.SetDirection(Vector2.zero);
        if (_current != null)
        {
            StopCoroutine(_current);
        }
        _current = StartCoroutine(coroutine);
    }
    public void OnDie()
    {
        _creature.SetDirection(Vector2.zero);
        _isDead = true;
        _animator.SetBool(IsDeadKey, true);

        _creature.SetDirection(Vector2.zero);
        if (_current != null)
        {
            StopCoroutine(_current);
        }
    }
}
