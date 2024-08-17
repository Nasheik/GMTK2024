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
        Debug.Log("Movement: " + inputVector);
        return inputVector;
    }
    public bool GetDashInput()
    {
        bool sprintInput = playerInputActions.Player.Sprint.WasPerformedThisFrame();
        Debug.Log("Sprint: " + sprintInput);
        return sprintInput;
    }
    public bool GetJumpInput()
    {
        bool jumpInput = playerInputActions.Player.Jump.WasPerformedThisFrame();
        Debug.Log("Jump: " + jumpInput);
        return jumpInput;
    }
    public Vector2 GetMouseDelta()
    {
        Vector2 mouseDelta = playerInputActions.Player.Aim.ReadValue<Vector2>();
        Debug.Log("Aim: " + mouseDelta);
        return mouseDelta;
    }
}
