using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Creatures.Weapons
{
    public class Projectile : BaseProjectile
    {
        
        protected override void Start()
        {
           base.Start();
            var foce = new Vector2(Direction * Speed, 0);
            Rigidbody.AddForce(foce, ForceMode2D.Impulse);
          
        }
       



    }
}

