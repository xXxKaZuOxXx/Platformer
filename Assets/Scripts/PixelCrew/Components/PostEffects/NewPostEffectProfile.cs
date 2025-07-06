using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class NewPostEffectProfile : MonoBehaviour
{
    [SerializeField] private VolumeProfile _profile;
    public void Set()
    {
        Volume[] volumes = FindObjectsOfType<Volume>();
        foreach (var volume in volumes)
        {
            if (!volume.isGlobal) continue;

            volume.profile = _profile;
        }
    }
}
