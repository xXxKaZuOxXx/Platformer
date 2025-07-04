using PixelCrew.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularProjectileSpawner : MonoBehaviour
{
    [SerializeField] CircularProjectileSettings[] _settings;

    public int Stage { get;  set; }

    [ContextMenu("Launch!")]
    public void LaunchProjectile()
    {
        StartCoroutine(SpawnProjectiles());
    }

    private IEnumerator SpawnProjectiles()
    {
        var setting = _settings[Stage];
        var sectorStep = 2 * Mathf.PI/ setting.BurstCount;
        for (int i = 0; i < setting.BurstCount; i++)
        {
            var angle = sectorStep * i;
            var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            var instance = SpawnUtils.Spawn(setting.Prefab.gameObject, transform.position);
            var projectile = instance.GetComponent<DirectionalProjectile>();
            projectile.Launch(direction);
            yield return new WaitForSeconds(setting.Delay);
        }

    }
    [Serializable]
    public struct CircularProjectileSettings
    {
        [SerializeField] private DirectionalProjectile _prefab;
        //[SerializeField] private int _itemsPerBurst;
        [SerializeField] private int _burstCount;
        [SerializeField] private float _delay;
        public DirectionalProjectile Prefab => _prefab;
        public int BurstCount => _burstCount;
        public float Delay => _delay; 
    }
}
