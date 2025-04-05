using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class Damage : MonoBehaviour
    {
        [SerializeField] private int _damage;
        public void ApplyDamage(GameObject target)
        {
            var health = target.GetComponent<Health>();
            if(health != null)
            {
                health.ApplyDamage(_damage);
            }

        }
    }

}

