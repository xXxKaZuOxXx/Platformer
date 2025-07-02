using PixelCrew.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroInputReader : MonoBehaviour
{
    [SerializeField]private Hero _hero;
    private float _startTime = 0;
    private float _holdDuration = 0;
    private float _firstClickTime;
    private bool _isFirstClick;

    private GameSession _session;
    private void Start()
    {
        _session = FindObjectOfType<GameSession>();
    }

    public void WASD(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        if (_isFirstClick && (Time.time - _firstClickTime > 0.5f))
        {
            _isFirstClick = false;
        }
        if (context.started && (direction == Vector2.left || direction == Vector2.right))
        {
            if(!_isFirstClick)
            {
                _firstClickTime = Time.time;
                _isFirstClick = true;
            }
            else 
            {
                float timeBetweenClicks = Time.time - _firstClickTime;
                if(timeBetweenClicks <= 0.5 && _session.PerksModel.IsDashSupported)
                {
                    _hero.OnDash();
                    Debug.Log("DO");
                }
                Debug.Log(timeBetweenClicks);
                _isFirstClick = false;
            }
           
            // Debug.Log(_startTime);

        }
        else
        {
            _hero.SetDirection(direction);
            
        }

            
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
                _hero.UseInventory(triple);
                
        }
        else if(_holdDuration < 3 && context.canceled)
        {
                bool triple = false;
                _hero.UseInventory(triple);
        }

        _holdDuration = 0;


    }
    public void OnHeal(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.canceled)
        {
            _hero.DoHeal();
        }
    }
    public void OnNextItem(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.canceled)
        {
            _hero.NextItem();
        }
    }
    public void OnUsePerk(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.canceled)
            _hero.UsePerk();
    }
    public void OnToggleFlashlight(InputAction.CallbackContext callbackContext)
    {
        if(callbackContext.performed)
            _hero.ToggleFlashlight();
    }

}
