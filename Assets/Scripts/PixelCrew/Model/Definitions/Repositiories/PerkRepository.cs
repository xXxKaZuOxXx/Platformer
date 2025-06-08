using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Defs/Repositories/Perks", fileName = "Perks")]
public class PerkRepository : DefRepository<PerkDef>
{
    public PerkDef[] Collection => _colletion;
}

[Serializable]
public struct PerkDef : IHaveId
{
    [SerializeField] private string _id;
    [SerializeField] private Sprite _icon;
    [SerializeField] public string _info;
    [SerializeField] private ItemWithCount _price;
    [SerializeField] private float _cooldown;
    public string Id => _id;
    public Sprite Icon => _icon;
    public string Info => _info;
    public ItemWithCount Price => _price;
    public float Cooldown => _cooldown;
}
[Serializable]
public struct ItemWithCount
{
    [InventoryId][SerializeField] private string _itemId;
    [SerializeField] private int _count;
    public int Count => _count;
    public string ItemId => _itemId;
}