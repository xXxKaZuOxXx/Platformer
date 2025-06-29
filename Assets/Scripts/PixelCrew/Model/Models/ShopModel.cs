using PixelCrew.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopModel: IDisposable
{
    public event Action OnChanged;

    private readonly PlayerData _data;
    public readonly StringProperty InterfaceSelection = new StringProperty();
    private readonly CompositeDisposable _trash = new CompositeDisposable();


    public ShopModel(PlayerData data)
    {
        _data = data;
        InterfaceSelection.Value = DefsFacade.I.Shop.All[0].Id;
        _trash.Retain(InterfaceSelection.Subscribe((x, y) => OnChanged?.Invoke()));
    }
    public void BuyItem(string id)
    {
        var def = DefsFacade.I.Shop.Get(id);
        var isEnoughResourses = _data.Inventory.IsEnough(def.Price);
        if (isEnoughResourses)
        {
            _data.Inventory.Remove(def.Price.ItemId, def.Price.Count);
            
            _data.Inventory.Add(id, 1);

            OnChanged?.Invoke();
        }
    }
    public IDisposable Subscribe(Action call)
    {
        OnChanged += call;
        return new ActionDisposable(() => OnChanged -= call);
    }
    public void Dispose()
    {
        _trash.Dispose();
    }

   
}
