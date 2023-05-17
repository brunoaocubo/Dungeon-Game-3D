using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public static class InputActionButton
{
    public static bool GetButton(this InputAction action) => action.ReadValue<float>() > 0;
    public static bool GetButtonDown(this InputAction action) => action.triggered && action.ReadValue<float>() > 0;
    public static bool GetButtonUp(this InputAction action) => action.triggered && action.ReadValue<float>() == 0;
}

public class Inputs : MonoBehaviour
{
    private PlayerInputActions inputActions;

    void Start()
    {
        inputActions = new PlayerInputActions();
        inputActions.Enable();
    }

    public Vector2 GetMovementVectorNormalized() 
    {
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }
    public bool GetAttackButton() 
    {
        bool pressFire1 = false;

        if(InputActionButton.GetButtonDown(inputActions.Player.Attack)) 
        {
            pressFire1 = true;
        }       
        return pressFire1;
    }

    public bool GetDodgeButton()
    {
        bool pressSpace = false;

        if (InputActionButton.GetButtonDown(inputActions.Player.Dodge))
        {
            pressSpace = true;
        }
        return pressSpace;
    }

    public bool GetEscapeButton()
    {
        bool pressEsc = false;

        if(InputActionButton.GetButtonDown(inputActions.Player.Configurations))
        {
            pressEsc = true;
        }
        else if(InputActionButton.GetButtonUp(inputActions.Player.Configurations))
        {
            pressEsc = false;
        }
        return pressEsc;
    }
    
    public bool GetUtilityButton1()
    {
        bool pressUtilityBtn1 = false;

        if(InputActionButton.GetButtonDown(inputActions.Player.Utility_1))
        {
            pressUtilityBtn1 = true;
        }
        return pressUtilityBtn1;
    }
}
