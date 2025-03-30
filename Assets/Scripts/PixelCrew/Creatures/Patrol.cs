using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Model
{
    public abstract class Patrol : MonoBehaviour
    {

        public abstract IEnumerator DoPatrol();
      
    }
}

