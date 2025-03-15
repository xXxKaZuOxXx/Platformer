using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class PlatformMove : MonoBehaviour
    {
        [SerializeField] private float _max_height;
        [SerializeField] private float _min_height;
        [SerializeField] private float _speed;
        [SerializeField] private Transform _transform;
        
       
        private bool _goOnTop = true;
        private bool _canMove = false;
        
     
       
        private void FixedUpdate()
        {
            if(_canMove)
            {
                if (transform.position.y <= _max_height && _goOnTop == true)
                {
                    var yDirection = transform.position.y + _speed * Time.deltaTime;
                    transform.position = new Vector3(transform.position.x, yDirection, transform.position.z);
                    if (transform.position.y >= _max_height)
                    {
                        _goOnTop = false;
                    }
                }
                else if (transform.position.y >= _min_height && _goOnTop == false)
                {
                    var yDirection = transform.position.y - _speed * Time.deltaTime;
                    transform.position = new Vector3(transform.position.x, yDirection, transform.position.z);
                    if (transform.position.y <= _min_height)
                    {
                        _goOnTop = true;
                    }
                }
            }
           
        }
        public void EnableMove()
        {
            _canMove = true;
        }
        protected void OnCollisionEnter2D(Collision2D collision)
        {
            collision.rigidbody.transform.SetParent(transform);
            

        }
        protected void OnCollisionExit2D(Collision2D collision)
        {
            collision.rigidbody.transform.SetParent(null);
        }
    }
}

