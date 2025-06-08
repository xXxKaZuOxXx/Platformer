using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Defs/Throwable", fileName = "Throwable")]
public class ThrowableRepository : DefRepository<ThrowableDef>
{
    //[SerializeField] private ThrowableDef[] _items;

    //private void OnEnable()
    //{
    //    _colletion = _items;
    //}
    //public ThrowableDef Get(string id)
    //{
    //    foreach (var item in _items)
    //    {
    //        if (item.Id == id)
    //            return item;
    //    }
    //    return default;
    //}
}

[Serializable]
public struct ThrowableDef: IHaveId
{
    [InventoryId][SerializeField] private string _id;
    [SerializeField] private GameObject _projectile;
    
    public string Id => _id;
    public GameObject Projectile => _projectile;
}
