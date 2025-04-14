﻿using PixelCrew.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RequireItem : MonoBehaviour
{
    [SerializeField] private InventoryItemData[] _required;
    //[InventoryId] [SerializeField] private string _id;
    //[SerializeField] private int _count;
    [SerializeField] private bool _removeAfterUse;

    [SerializeField] private UnityEvent _onSuccess;
    [SerializeField] private UnityEvent _onFail;

    public void Check()
    {
        var session = FindObjectOfType<GameSession>();
        var isAllRequirements = true;
        foreach ( var item in _required)
        {
            var numItems = session.Data.Inventory.Count(item.Id);
            if(numItems < item.Value )
                isAllRequirements = false;
        }
        
        if (isAllRequirements)
        {
            if (_removeAfterUse)
            {
                foreach ( var item in _required)
                {
                    session.Data.Inventory.Remove(item.Id, item.Value);
                }
            }    

            _onSuccess?.Invoke();
        }
        else
        {
            _onFail?.Invoke();
        }

    }
}
