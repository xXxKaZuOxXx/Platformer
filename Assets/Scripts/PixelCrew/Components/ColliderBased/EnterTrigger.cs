using PixelCrew.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace PixelCrew.Components
{
    public class EnterTrigger : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private LayerMask _layer = ~0;
        [SerializeField] private EnterEvent _action;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(!collision.gameObject.IsInLayer(_layer)) return;
            if(!string.IsNullOrEmpty(_tag) && !collision.gameObject.CompareTag(_tag)) return;

            _action?.Invoke(collision.gameObject);
            
        }
        

    }
}

