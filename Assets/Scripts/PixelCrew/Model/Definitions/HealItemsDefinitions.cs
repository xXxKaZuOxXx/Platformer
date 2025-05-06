using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Defs/HealItemsDef", fileName = "HealItems")]
public class HealItemsDefinitions : ScriptableObject
{
    [SerializeField] private HealDef[] _items;

    public HealDef Get(string id)
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
public struct HealDef
{
    [InventoryId][SerializeField] private string _id;
    [SerializeField] private GameObject _prefab;

    public string ID => _id;
    public GameObject Prefab => _prefab;
}
