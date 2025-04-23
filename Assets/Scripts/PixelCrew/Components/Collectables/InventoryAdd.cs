using PixelCrew.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryAdd : MonoBehaviour
{
    [InventoryId][SerializeField] private string _id;
    [SerializeField] private int _count;

    public void Add(GameObject go)
    {
        var hero = go.GetInterface<IcanAddInInventory>();
        hero?.AddInInventory(_id, _count);
        
    }
}
