using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalLevitation : MonoBehaviour
{
    [SerializeField] private float _frquency = 1f;
    [SerializeField] private float _amplitude = 1f;
    [SerializeField] private bool _randomize;

    private Rigidbody2D _rigidbody;
    private float _originalY;
    private float _seed;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _originalY = _rigidbody.transform.position.y;
        if (_randomize)
            _seed = Random.value * Mathf.PI * 2;
    }

    private void Update()
    {
        var pos = _rigidbody.position;
        pos.y = _originalY + Mathf.Sin(_seed + Time.time * _frquency) * _amplitude;
        _rigidbody.MovePosition(pos);
    }
}
