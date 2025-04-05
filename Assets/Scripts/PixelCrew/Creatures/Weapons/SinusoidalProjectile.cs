using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusoidalProjectile : BaseProjectile
{
    [SerializeField] private float _frquency = 1f;
    [SerializeField] private float _amplitude = 1f;

    private float _originalY;
    private float _time;
    

    protected override void Start()
    {

        base.Start();
        _originalY = Rigidbody.position.y;
    }
    protected void FixedUpdate()
    {
        var position = Rigidbody.position;
        position.x += Direction * Speed;
        position.y = _originalY + Mathf.Sin(_time * _frquency) * _amplitude;
        Rigidbody.MovePosition(position);
        _time += Time.fixedDeltaTime;

    }

}
