using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input : MonoBehaviour
{
    private PlayerInputActions playerInputActions;



    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>().normalized;
        return inputVector;
    }
    public bool GetDashInput()
    {
        bool sprintInput = playerInputActions.Player.Sprint.IsPressed();
        return sprintInput;
    }
    public bool GetJumpInput()
    {
        bool jumpInput = playerInputActions.Player.Jump.IsPressed();
        return jumpInput;
    }
    public Vector2 GetMouseDelta()
    {
        Vector2 mouseDelta = playerInputActions.Player.Aim.ReadValue<Vector2>();
        return mouseDelta;
    }
}
