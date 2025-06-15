using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class Spawn : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private bool _invertXScale;

        [ContextMenu("SpawnTarget")]
        public void SpawnTarget()
        {

            SpawnInstance();
        }
        public GameObject SpawnInstance()
        {
            var instantiate = Instantiate(_prefab, _target.position, Quaternion.identity);
            var scale = _target.lossyScale;
            scale.x *= _invertXScale ? -1 : 1;
            instantiate.transform.localScale = scale;
            return instantiate;
        }
        internal void SetPrefab(GameObject prefab)
        {
            _prefab = prefab;
        }
    }

}

