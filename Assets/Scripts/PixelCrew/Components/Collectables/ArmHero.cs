using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class ArmHero : MonoBehaviour
    {
        
        public void ArmedHero(GameObject go)
        {
            var hero = go.GetComponent<Hero>();
            if (hero != null) 
            {
                hero.ArmedHero();
            }
        }
    }
}

