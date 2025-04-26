using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomButton : Button
{
    [SerializeField] private GameObject _normal;
    [SerializeField] private GameObject _select;

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);
        _normal.SetActive(state != SelectionState.Pressed);
        _select.SetActive(state == SelectionState.Pressed);
         
    }
}
