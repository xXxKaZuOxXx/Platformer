using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryData 
{
    [SerializeField] private List<InventoryItemData> _inventory = new List<InventoryItemData>();

    public delegate void OnInventoryChanged(string id, int value);

    public OnInventoryChanged OnChanged;

    public void Add(string id, int value)
    {
        if(value <= 0) return;

        var itemDef = DefsFacade.I.Items.Get(id);
        if(itemDef.IsVoid) return;

        var item = GetItem(id);
        if(itemDef.IsSingle && _inventory.Count < 6)
        {
            item = new InventoryItemData(id);
            item.Value = 1;
            _inventory.Add(item);
        }
        else if(item == null )
        {
            item = new InventoryItemData(id);
            _inventory.Add(item);
        }
        if(!itemDef.IsSingle)
            item.Value += value;

        OnChanged?.Invoke(id, Count(id));
    }
    public void Remove(string id, int value)
    {
        var itemDef = DefsFacade.I.Items.Get(id);
        if (itemDef.IsVoid) return;

        var item = GetItem(id);
        if (item == null) return;

        item.Value -= value;

        if(item.Value <= 0)
            _inventory.Remove(item);

        OnChanged?.Invoke(id, Count(id));
    }

    public int Count(string id)
    {
        var count = 0;
        foreach (var item in _inventory)
        {
            if (item.Id == id)
                count += item.Value;
        }
        return count;
    }

    private InventoryItemData GetItem(string id)
    {
        foreach(InventoryItemData item in _inventory)
        {
            if(item.Id == id) return item;
        }
        return null;
    }
}

[Serializable]
public class InventoryItemData
{
    [InventoryId] public string Id;
    public int Value;
    //public bool IsSingle = false;

    public InventoryItemData(string id) 
    {
        Id = id;
        
    }
}