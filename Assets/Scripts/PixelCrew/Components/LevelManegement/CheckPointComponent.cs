using PixelCrew.Components;
using PixelCrew.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Spawn))]
public class CheckPointComponent : MonoBehaviour
{
    [SerializeField] private string _id;
    [SerializeField] private UnityEvent _setChecked;
    [SerializeField] private UnityEvent _setUnchecked;
    [SerializeField] private Spawn _heroSpawner;

    private GameSession _session;
   

    public string Id => _id;
    private void Awake()
    {
        _heroSpawner = GetComponent<Spawn>();
    }
    private void Start()
    {
        
        _session = FindObjectOfType<GameSession>();
        if(_session.IsChecked(_id))
        {
            _setChecked?.Invoke();
        }
        else
        {
            _setUnchecked?.Invoke();
        }
    }
    public void Check()
    {
        _session.SetChecked(_id);
        _setChecked?.Invoke();
    }
    public void SpawnHero()
    {
        _heroSpawner.SpawnTarget();
    }
}
