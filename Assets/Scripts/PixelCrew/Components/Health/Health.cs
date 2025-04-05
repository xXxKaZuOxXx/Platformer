using System;
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
        [SerializeField] private HealthChangeEvent _onChange;
        private bool dead = false;
        public int HealthValue { get { return _health; } }

        public void ApplyDamage(int damageValue)
        {
            _health -= damageValue;
            _onChange?.Invoke(_health);
            if (_health <= 0 && !dead)
            {
                dead = true;
                _onDie?.Invoke();
            }
            else if (damageValue > 0 && !dead)
            {
                _onDamage?.Invoke();
            }
          
           
        }

        internal void SetHealth(int hp)
        {
            _health = hp;
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
    [Serializable]
    public class HealthChangeEvent: UnityEvent<int>
    {

    }

}
