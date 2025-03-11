using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class Teleport : MonoBehaviour
    {
        [SerializeField] private Transform _destTransform;
        
        public void TeleportTarget(GameObject target)
        {
            target.transform.position = _destTransform.position;
        }
    }
}

