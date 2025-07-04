using PixelCrew.Creatures.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalProjectile : BaseProjectile
{
    public void Launch(Vector2 direction)
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Rigidbody.AddForce(direction*Speed, ForceMode2D.Impulse);
    }
}
