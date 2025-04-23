using PixelCrew.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour, IcanAddInInventory
{
    [SerializeField] private List<InventoryItemData> _items = new List<InventoryItemData>();
    public void AddInInventory(string id, int value)
    {
        _items.Add(new InventoryItemData(id){Value = value});
    }
    public void DropInInventory()
    {
        var session = FindObjectOfType<GameSession>();
        foreach (var item in _items)
        {
            session.Data.Inventory.Add(item.Id, item.Value); 
        }
        _items.Clear();
    }
}
