using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
}
