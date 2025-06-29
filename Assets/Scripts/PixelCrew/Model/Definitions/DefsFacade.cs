using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")]
public class DefsFacade : ScriptableObject
{
    [SerializeField] private ItemsRepository _items;
    [SerializeField] private PlayerDef _playerDef;
    [SerializeField] private ThrowableRepository _throwable;
    [SerializeField] private HealItemsDefinitions _healItems;
    [SerializeField] private PoitionRepository _poitions;
    [SerializeField] private PerkRepository _perks;
    [SerializeField] private ShopRepository _shop;

    public ItemsRepository Items => _items;
    public PlayerDef PlayerDef => _playerDef;
    public PerkRepository Perks => _perks;
    public ShopRepository Shop => _shop;
    public ThrowableRepository Throwable => _throwable;
    public HealItemsDefinitions HealItems => _healItems;
    public PoitionRepository Poitions => _poitions;

    private static DefsFacade _instance;
    public static DefsFacade I => _instance == null? LoadDefs(): _instance;

    private static DefsFacade LoadDefs()
    {
        return _instance = Resources.Load<DefsFacade>("DefsFacade");
    }

    
}
