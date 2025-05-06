using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Defs/ThrowableItemsDef", fileName = "ThrowableItems")]
public class ThrowableItemsDef : ScriptableObject
{
    [SerializeField] private ThrowableDef[] _items;

    public ThrowableDef Get(string id)
    {
        foreach (var item in _items)
        {
            if (item.ID == id)
                return item;
        }
        return default;
    }
}

[Serializable]
public struct ThrowableDef
{
    [InventoryId][SerializeField] private string _id;
    [SerializeField] private GameObject _projectile;
    
    public string ID => _id;
    public GameObject Projectile => _projectile;
}
