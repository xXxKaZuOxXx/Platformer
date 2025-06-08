using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class AbstractLocalizeComponent: MonoBehaviour
{
    protected virtual void Awake()
    {
        

        LocalisationManager.I.OnLocalChanged += OnLocalChanged;
        Localise();
    }

    private void OnLocalChanged()
    {
        Localise();
    }
    private void OnDestroy()
    {
        LocalisationManager.I.OnLocalChanged -= OnLocalChanged;
    }
    protected abstract  void Localise();
}
