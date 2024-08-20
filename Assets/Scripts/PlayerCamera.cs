using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerCamera : MonoBehaviour
{

    public float xSensitivity = 40;
    public float ySensitivity = 40;
    public Transform orientation;
    private float xRotation;
    private float yRotation;

    public Input input;

    private void Start()
    {
        LockMouse();
    }

    private void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame) LockMouse();
        if (Keyboard.current.eKey.wasPressedThisFrame) UnlockMouse();

        if (!input) return;
        Vector2 aimInput = input.GetMouseDelta();
        if (aimInput != Vector2.zero) AttemptAim(aimInput);
    }

    public void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void AttemptAim(Vector2 mouseDelta)
    {
        if (!orientation) return;
        yRotation += mouseDelta.x * Time.deltaTime * xSensitivity;
        xRotation -= mouseDelta.y * Time.deltaTime * ySensitivity;

        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
