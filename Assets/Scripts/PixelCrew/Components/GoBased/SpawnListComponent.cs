﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace PixelCrew.Components
{
    public class SpawnListComponent : MonoBehaviour
    {
        [SerializeField] private SpawnData[] _spawners;

      

        public void Spawn(string id)
        {
            var spawner =_spawners.FirstOrDefault(element => element.Id == id);
            spawner?.Component.SpawnTarget();
        }
    }

    [Serializable]
    public class SpawnData
    {
        public string Id;
        public Spawn Component;
    }
}

