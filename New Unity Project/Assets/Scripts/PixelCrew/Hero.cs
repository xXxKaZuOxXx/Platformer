using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    private Vector2 _direction;

    [SerializeField]private float _speed;
    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }
    private void Update()
    {
        //if(_direction.x != 0 || _direction.y != 0)
        //{
        //    var deltaX = _direction.x * _speed * Time.deltaTime;
        //    var newXPosition = transform.position.x + deltaX;
        //    var deltaY = _direction.y * _speed * Time.deltaTime;
        //    var newYPosition = transform.position.y + deltaY;
        //    transform.position = new Vector3(newXPosition, newYPosition, transform.position.z);
        //}
        if (_direction.x != 0)
        {
            var delta = _direction * _speed * Time.deltaTime;
            var newXPosition = transform.position.x + delta.x;
            transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);
        }
        if (_direction.y != 0)
        {
            var delta = _direction * _speed * Time.deltaTime;
            var newYPosition = transform.position.y + delta.y;
            transform.position = new Vector3(transform.position.x, newYPosition, transform.position.z);
        }

    }
    public void SaySomething()
    {
        Debug.Log("Some");
    }
}
