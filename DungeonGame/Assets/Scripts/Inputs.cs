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
        bool isAttack = false;

        if(InputActionButton.GetButtonDown(inputActions.Player.Attack)) 
        {
            isAttack = true;
        }       
        return isAttack;
    }

    public bool GetDodgeButton()
    {
        bool isAttack = false;

        if (InputActionButton.GetButtonDown(inputActions.Player.Dodge))
        {
            isAttack = true;
        }
        return isAttack;
    }

}
