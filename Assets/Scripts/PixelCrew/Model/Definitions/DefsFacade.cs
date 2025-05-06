using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")]
public class DefsFacade : ScriptableObject
{
    [SerializeField] private InventoryItemsDefinitions _items;
    [SerializeField] private PlayerDef _playerDef;
    [SerializeField] private ThrowableItemsDef _throwable;
    [SerializeField] private HealItemsDefinitions _healItems;

    public InventoryItemsDefinitions Items => _items;
    public PlayerDef PlayerDef => _playerDef;
    public ThrowableItemsDef Throwable => _throwable;
    public HealItemsDefinitions HealItems => _healItems;

    private static DefsFacade _instance;
    public static DefsFacade I => _instance == null? LoadDefs(): _instance;

    private static DefsFacade LoadDefs()
    {
        return _instance = Resources.Load<DefsFacade>("DefsFacade");
    }

    
}
