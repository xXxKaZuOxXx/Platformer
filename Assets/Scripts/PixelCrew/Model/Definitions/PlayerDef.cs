using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Defs/PlayerDef", fileName = "PlayerDef")]
public class PlayerDef : ScriptableObject
{
    [SerializeField] private int _inventorySize;
    [SerializeField] private int _maxHealth;
    [SerializeField] private StatDef[] _stats;
    
    public StatDef[] Stats => _stats;
    public int InventorySize => _inventorySize;
    public int MaxHealth => _maxHealth;
    
    public StatDef GetStat(StatId id) => _stats.FirstOrDefault(x => x.Id == id);
}
