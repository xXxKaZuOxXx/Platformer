using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Defs/Poitions", fileName = "Poitions")]
public class PoitionRepository : DefRepository<PoitionDef>
{

}

[Serializable]
public struct PoitionDef : IHaveId
{
    [InventoryId][SerializeField] private string _id;
    [SerializeField] private float _value;
    [SerializeField] private float _time;
    [SerializeField] private Effect _effect;
    public string Id => _id;
    public float Value => _value;
    public float Time => _time;
    public Effect Effect => _effect;
}

public enum Effect
{
    AddHp,
    SpeedUp
}

