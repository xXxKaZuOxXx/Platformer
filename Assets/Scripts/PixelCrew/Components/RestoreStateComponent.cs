using PixelCrew.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreStateComponent : MonoBehaviour
{
    
    [SerializeField] private string _id;
    public string Id => _id;

    
    private GameSession _session;
    

    

    private void Start()
    {
        _session = FindObjectOfType<GameSession>();
        var IsDestroyed = _session.RestoreState(Id);
        if(IsDestroyed)
            Destroy(gameObject);
    }

   

}
