using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputEnableComponent : MonoBehaviour
{
    private PlayerInput _input;

    // Start is called before the first frame update
    void Start()
    {
        _input = FindObjectOfType<PlayerInput>();
    }

    public void SetInput(bool isEnabled)
    {
        _input.enabled = isEnabled;
    }
    
}
