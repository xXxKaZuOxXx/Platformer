using PixelCrew.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopWindow : AnimatedWindow
{
    [SerializeField] private Button _buyButton;
    [SerializeField] private ItemWidget _price;
    [SerializeField] private Transform _itemsContainer;

    private PredefinedDataGroup<ShopDef, ShopWidget> _dataGroup;
    private readonly CompositeDisposable _trash = new CompositeDisposable();
    private GameSession _session;

    protected override void Start()
    {
        base.Start();

        _dataGroup = new PredefinedDataGroup<ShopDef, ShopWidget>(_itemsContainer);
        _session = FindObjectOfType<GameSession>();

        _trash.Retain(_buyButton.onClick.Subscribe(OnBuy));
        _trash.Retain(_session.ShopModel.Subscribe(OnSelectChanged));
       
        OnSelectChanged();
       
    }

    private void OnSelectChanged()
    {
        _dataGroup.SetData(DefsFacade.I.Shop.All);
        var selected = _session.ShopModel.InterfaceSelection.Value;

        var def = DefsFacade.I.Shop.Get(selected);
        _price.SetData(def.Price);
    }

    private void OnBuy()
    {
        var selected = _session.ShopModel.InterfaceSelection.Value;
        _session.ShopModel.BuyItem(selected);
    }
    private void OnDestroy()
    {
        _trash.Dispose();
    }
}
