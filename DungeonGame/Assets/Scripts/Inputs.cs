using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
