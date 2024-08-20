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
    public int GetGrappleInput()
    {
        int grappleInput = playerInputActions.Player.Grapple.WasPressedThisFrame() ? 1 : playerInputActions.Player.Grapple.WasReleasedThisFrame() ? -1 : 0;
        return grappleInput;
    }
    public int GetSwingInput()
    {
        int swingInput = playerInputActions.Player.Swing.WasPressedThisFrame() ? 1 : playerInputActions.Player.Swing.WasReleasedThisFrame() ? -1 : 0;
        return swingInput;
    }

    public bool GetPauseInput()
    {
        bool input = playerInputActions.Player.Pause.WasPressedThisFrame();
        return input;
    }
    public bool GetRestartInput()
    {
        bool input = playerInputActions.Player.Restart.WasPressedThisFrame();
        return input;
    }
    public bool GetFRestartInput()
    {
        bool input = playerInputActions.Player.FRestart.WasPerformedThisFrame();
        return input;
    }
}
