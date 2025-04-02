using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Creatures.Weapons
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed;

        private Rigidbody2D _rigidbody;
        private int _direction;
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _direction = transform.lossyScale.x > 0 ? 1 : -1;
            var foce = new Vector2(_direction * _speed, 0);
            _rigidbody.AddForce(foce, ForceMode2D.Impulse);
        }

    //    private void FixedUpdate()
    //    {
    //        var position = _rigidbody.position;
    //        position.x += _speed * _direction;
    //        _rigidbody.MovePosition(position);
    //    }
    }
}

