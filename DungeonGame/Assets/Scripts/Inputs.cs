using System;
using UnityEngine;
using UnityEngine.InputSystem;
<<<<<<< HEAD

=======
using UnityEngine.UI;
public static class InputActionButton
{
    public static bool GetButton(this InputAction action) => action.ReadValue<float>() > 0;
    public static bool GetButtonDown(this InputAction action) => action.triggered && action.ReadValue<float>() > 0;
    public static bool GetButtonUp(this InputAction action) => action.triggered && action.ReadValue<float>() == 0;
}
>>>>>>> b2bf63e9ea4b7ee615659e2f02965ba0d8b2c647

public class Inputs : MonoBehaviour
{
    private PlayerInputActions inputActions;
    public event EventHandler OnInteractAction;
    public event EventHandler OnAttackAction;
    public event EventHandler OnDodgeAction;

    void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Enable();
        inputActions.Player.Interact.performed += Interact_performed;
        inputActions.Player.Attack.performed += Attack_performed;
        inputActions.Player.Dodge.performed += Dodge_performed;
    }

    private void Dodge_performed(InputAction.CallbackContext obj)
    {
        OnDodgeAction?.Invoke(this, EventArgs.Empty);
    }

    private void Attack_performed(InputAction.CallbackContext obj)
    {
        OnAttackAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized() 
    {
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }
<<<<<<< HEAD
=======
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
>>>>>>> b2bf63e9ea4b7ee615659e2f02965ba0d8b2c647
}
