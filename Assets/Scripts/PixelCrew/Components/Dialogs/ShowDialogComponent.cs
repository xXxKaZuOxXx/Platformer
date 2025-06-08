using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField] private UnityEvent _onComplete;
    //[SerializeField] public string[] LocalisationKeys;

    private DialogBoxController _dialogBox;
    public void Show()
    {
        _dialogBox = FindDialogController();
        
         
        _dialogBox.ShowDialog(Data,_onComplete);
    }

    private DialogBoxController FindDialogController()
    {
        if(_dialogBox != null) return _dialogBox;

        GameObject controllerGo = null;
        switch (Data.Type)
        {
            case DialogType.Simple:
                controllerGo = GameObject.FindWithTag("SimpleDialog");
                break;
            case DialogType.Personalized:
                controllerGo = GameObject.FindWithTag("PersonalizedDialog");
                break;
            default:
                throw new ArgumentException("Undefided dialog type");
        }
        return controllerGo.GetComponent<DialogBoxController>();
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

