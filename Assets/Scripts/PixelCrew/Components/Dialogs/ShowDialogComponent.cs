using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDialogComponent : MonoBehaviour
{
    public enum Mode
    {
        Bound,
        External
    }
    [SerializeField] public Mode _mode;
    [SerializeField] public DialogData _bound;
    [SerializeField] public DialogDef _external;
    //[SerializeField] public string[] LocalisationKeys;

    private DialogBoxController _dialogBox;
    public void Show()
    {
        if (_dialogBox == null)
        {
            _dialogBox = FindObjectOfType<DialogBoxController>();
        }
        _dialogBox.ShowDialog(Data);
    }
    public DialogData Data
    {
        get
        {
            switch (_mode)
            {
                case Mode.Bound:
                    return _bound;
                case Mode.External:
                    return _external.Data;
                default:
                    throw new ArgumentOutOfRangeException();

            }
        }
    }

    public void Show(DialogDef def)
    {
        _external = def;
        Show();
    }
}

