using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

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
            if (_health <= 0)
            {
                _onDie?.Invoke();
            }
            else if (damageValue > 0)
            {
                _onDamage?.Invoke();
            }
          
           
        }
        public static bool operator <(Health counter1,int value)
        {
            return counter1._health < value;
        }
        public static bool operator >(Health counter1, int value)
        {
            return counter1._health > value;
        }
    }

}
