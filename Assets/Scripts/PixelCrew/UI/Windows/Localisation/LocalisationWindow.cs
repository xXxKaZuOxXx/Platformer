using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalisationWindow : AnimatedWindow
{
    [SerializeField] private Transform _container;
    [SerializeField] private LocalItemWidget _prefab;

    private DataGroup<LocalInfo, LocalItemWidget> _dataGroup;
    private readonly string[] _supportedLocals = new[] { "en", "ru" };

    protected override void Start()
    {
        base.Start();
        _dataGroup = new DataGroup<LocalInfo, LocalItemWidget>(_prefab,_container);
        _dataGroup.SetData(ComposeData());
        

    }
    
    private List<LocalInfo> ComposeData()
    {
        var data = new List<LocalInfo>();
        foreach (var item in _supportedLocals)
        {
            data.Add(new LocalInfo { LocalId = item });
        }
        return data;
    }

    public void OnSelected(string selectedLocal)
    {
        LocalisationManager.I.SetLocal(selectedLocal);
    }
}
