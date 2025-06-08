using PixelCrew.Model;
using PixelCrew.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
//using static ObservableProperty<StringProperty>;

public class PerksModel: IDisposable
{
    private readonly PlayerData _data;
    public readonly StringProperty InterfaceSelection = new StringProperty();
    public readonly Cooldown Cooldown = new Cooldown();
    private  readonly CompositeDisposable _trash = new CompositeDisposable();
    public bool IsSupperThrowSupported => _data.Perks.Used.Value == "super-throw" && Cooldown.IsReady;
    public bool IsDubleJumpSupported => _data.Perks.Used.Value == "double-jump" && Cooldown.IsReady;
    public bool IsShieldSupported => _data.Perks.Used.Value == "shield" && Cooldown.IsReady;
    

    public event Action OnChanged;
    public string Used => _data.Perks.Used.Value;

    public PerksModel(PlayerData data)
    {
        _data = data;
        InterfaceSelection.Value = DefsFacade.I.Perks.All[0].Id;

        _trash.Retain(_data.Perks.Used.Subscribe((x,y)=> OnChanged?.Invoke()));
        _trash.Retain(InterfaceSelection.Subscribe((x,y)=> OnChanged?.Invoke()));
    }
    public void Unlock(string id)
    {
        var def = DefsFacade.I.Perks.Get(id);
        var isEnoughResourses = _data.Inventory.IsEnough(def.Price);

        if (isEnoughResourses)
        {
            _data.Inventory.Remove(def.Price.ItemId, def.Price.Count);
            _data.Perks.AddPerk(id);

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

    public void SelectPerk(string selected)
    {
        var perkDef = DefsFacade.I.Perks.Get(selected);
        Cooldown.Value = perkDef.Cooldown;
        _data.Perks.Used.Value = selected;
    }

    public bool IsUsed(string perkId)
    {
        return _data.Perks.Used.Value == perkId;
    }

    public bool IsUnlocked(string perkId)
    {
        return _data.Perks.IsUnlocked(perkId);
    }

    internal bool CanBuy(string perkId)
    {
        var def = DefsFacade.I.Perks.Get(perkId);
        return _data.Inventory.IsEnough(def.Price);
    }
}
