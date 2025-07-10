using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class Damage : MonoBehaviour
    {
        [SerializeField] private int _damage;
        private int _originalDamage;
        private Lock _ultraDamage = new Lock();
        public int DamageValue => _damage;
        public Lock UltraDamage => _ultraDamage;
        private void Start()
        {
            _originalDamage = _damage;
        }
        public void SetDelta(int delta)
        {
            _damage = delta;
        }
        public void ApplyDamage(GameObject target)
        {
            var health = target.GetComponent<Health>();
            if(UltraDamage.IsLocked)
            {
                _damage = 10;
            }
            else
            {
                _damage = _originalDamage;
            }
            if(health != null)
            {
                health.ApplyDamage(_damage);
            }

        }
    }

}

