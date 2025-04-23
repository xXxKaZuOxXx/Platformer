using PixelCrew.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Creatures
{
    
    public class PlatformPatrol : Patrol
    {
        [SerializeField] private Collider2D _platform;
        private Collider2D _player;

        private Creature _creature;

        private void Awake()
        {
            _player = GetComponent<Collider2D>();
            _creature = GetComponent<Creature>();
        }
        private void Start()
        {
            
        }
        public override IEnumerator DoPatrol()
        {
            _creature.SetDirection(Vector2.right.normalized);
            while (enabled)
            {
                
                Vector2 platformSize = _platform.bounds.size;
                Vector2 platformCenter = _platform.bounds.center;
                Vector2 Left = new Vector2(platformCenter.x - platformSize.x / 2, platformCenter.y + platformSize.y / 2);
                Vector2 Right = new Vector2(platformCenter.x + platformSize.x / 2, platformCenter.y + platformSize.y / 2);
                Vector2 playerCenter = _player.bounds.center;
                Vector2 playerSize = _player.bounds.size;
                Vector2 LeftRay = new Vector2(playerCenter.x - playerSize.x / 2, playerCenter.y);
                Vector2 RigtRay = new Vector2(playerCenter.x + playerSize.x / 2, playerCenter.y);
                int layerMask = 1 << 8;
                var leftRaycast = Physics2D.Raycast(LeftRay, Vector2.down, 2, layerMask);
                var rightRaycast = Physics2D.Raycast(RigtRay, Vector2.down, 2, layerMask);
               
                if (leftRaycast && !rightRaycast)
                {
                    var dir = Vector2.left;
                    _creature.SetDirection(dir.normalized);
                }
                else if (!leftRaycast && rightRaycast)
                {
                    var dir = Vector2.right;
                    _creature.SetDirection(dir.normalized);
                }

                Debug.DrawRay(LeftRay, Vector2.down, Color.white);
                Debug.DrawRay(RigtRay, Vector2.down, Color.white);
                yield return null;
            }
        }

        private void CheckBounds()
        {
            
        }
    }

}
