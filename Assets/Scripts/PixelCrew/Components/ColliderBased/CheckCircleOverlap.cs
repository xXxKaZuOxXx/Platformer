using PixelCrew.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew
{
    public class CheckCircleOverlap : MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;
        [SerializeField] private string[] _tags;
        [SerializeField] private OnOverlapEvent _onOverlap;
        [SerializeField] private LayerMask _mask;
        private Collider2D[] _interactionResult = new Collider2D[5];

       

        internal void Check()
        {
            var size = Physics2D.OverlapCircleNonAlloc(transform.position, _radius, _interactionResult, _mask);

            var overlaps = new List<GameObject>();
            for (int i = 0; i < size; i++)
            {
                var overlapResult = _interactionResult[i];
                var isInTags =  _tags.Any(tag => overlapResult.CompareTag(tag));
                if(isInTags)
                {
                    _onOverlap?.Invoke(overlapResult.gameObject); 
                }
                
                
            }
        }
#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = HandlesUtils.TransparentRed;
            UnityEditor.Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius); 
        }
#endif
    }

    [Serializable]

    public class OnOverlapEvent: UnityEvent<GameObject>
    {

    }
    

}

