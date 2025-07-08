using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

namespace PixelCrew.Components
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] public UnityEvent _onDamage;
        [SerializeField] public UnityEvent _onDie;
        [SerializeField] public HealthChangeEvent _onChange;
        private Lock _immune = new Lock();
        private bool dead = false;
        public int HealthValue { get { return _health; } }

        public Lock Immune => _immune; 
        

        public void ApplyDamage(int damageValue)
        {
            if (damageValue > 0 && Immune.IsLocked)
                return;

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
