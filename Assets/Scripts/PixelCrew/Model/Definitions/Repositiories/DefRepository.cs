using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefRepository<TDefType> : ScriptableObject where TDefType : IHaveId
{
    [SerializeField] protected TDefType[] _colletion;

    public TDefType Get(string id)
    {
        if (string.IsNullOrEmpty(id))
            return default;
        foreach (var item in _colletion)
        {
            if (item.Id == id)
                return item;
        }
        return default;
    }

    public TDefType[] All => new List<TDefType>(_colletion).ToArray();
}
