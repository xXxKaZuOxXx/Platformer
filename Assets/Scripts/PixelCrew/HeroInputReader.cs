using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroInputReader : MonoBehaviour
{
    [SerializeField]private Hero _hero;
    private float _startTime = 0;
     private float _holdDuration = 0;



    public void WASD(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        _hero.SetDirection(direction);
    }
    public void OnVerticalMovement(InputAction.CallbackContext context)
    {
        
    }
    public void OnSaySomething(InputAction.CallbackContext context)
    {
        if(context.canceled)
            _hero.SaySomething();

        
      
    }
    public void OnInteract(InputAction.CallbackContext context)
    {

        if (context.canceled)
            _hero.Interact();

    }
    public void OnAttacking(InputAction.CallbackContext context)
    {

        if (context.canceled)
            _hero.Attack();
    }
    public void OnThrow(InputAction.CallbackContext context)
    {
        
     
        if (context.started)
        {
            _startTime = (float)context.time;
           // Debug.Log(_startTime);

        }
        if (context.canceled)
        {
            _holdDuration = (float)context.time - _startTime;
            //Debug.Log();
            //Debug.Log(holdDuration);
        }
        Debug.Log("START");
        Debug.Log(_startTime);
        Debug.Log("HOLD");
        Debug.Log(_holdDuration);
        

        if (_holdDuration != 0 && _holdDuration > 3)
        {
                bool triple = true;
                _hero.Throw(triple);
                
        }
        else if(_holdDuration < 3 && context.canceled)
        {
                bool triple = false;
                _hero.Throw(triple);
        }

        _holdDuration = 0;


    }

}
