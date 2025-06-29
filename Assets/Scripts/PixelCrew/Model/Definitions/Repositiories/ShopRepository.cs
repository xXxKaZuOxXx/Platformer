using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Defs/Repositories/Shop", fileName = "Shop")]
public class ShopRepository : DefRepository<ShopDef>
{
    public ShopDef[] Collection => _colletion;
}

[Serializable]
public struct ShopDef : IHaveId
{
    [SerializeField] private string _id;
    [SerializeField] private Sprite _icon;
    [SerializeField] private ItemWithCount _price;
    public string Id => _id;
    public Sprite Icon => _icon;
    public ItemWithCount Price => _price;

}
