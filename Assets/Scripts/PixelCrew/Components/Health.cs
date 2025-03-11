using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onDie;
        public void ApplyDamage(int damageValue)
        {
            _health -= damageValue;
            if(damageValue > 0)
            {
                _onDamage?.Invoke();
            }
          
            if(_health <=0)
            {
                _onDie?.Invoke();
            }
        }
    }

}
