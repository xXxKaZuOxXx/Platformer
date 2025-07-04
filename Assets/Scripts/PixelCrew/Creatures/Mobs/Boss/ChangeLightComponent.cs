using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Experimental.Rendering.Universal;

public class ChangeLightComponent : MonoBehaviour
{
    [SerializeField] private Light2D[] _lights;

    [ColorUsage(true, true)] [SerializeField] 
    private Color _color;

    [ContextMenu("Setup")]
    public void SetColor()
    {
        foreach (Light2D light in _lights)
        {
            light.color = _color;
        }
    }

}
