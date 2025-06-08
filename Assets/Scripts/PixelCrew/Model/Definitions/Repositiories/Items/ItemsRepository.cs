using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Defs/Items", fileName = "Items")]
public class ItemsRepository : DefRepository<ItemDef>
{
    //[SerializeField] private ItemDef[] _items;
    //private void OnEnable()
    //{
    //    _colletion = _items;
    //}
    //public ItemDef Get(string id)
    //{
    //    foreach (var item in _items)
    //    {
    //        if (item.Id == id)
    //            return item;
    //    }
    //    return default;
    //}
#if UNITY_EDITOR
    public ItemDef[] ItemsForEditor => _colletion;
#endif
}

[Serializable]
public struct ItemDef: IHaveId
{
    [SerializeField] private string _id;
    [SerializeField] private ItemTag[] _tags;
    [SerializeField] private Sprite _icon;
    public string Id => _id;
    
    public bool IsVoid => string.IsNullOrEmpty(_id);

    public Sprite Icon => _icon;

    public bool HasTag(ItemTag tag)
    {
        return _tags?.Contains(tag) ?? false;
    }
    
}
