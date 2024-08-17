using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame) Application.Quit();
        if (Keyboard.current.rKey.wasPressedThisFrame) SceneManager.LoadScene(0);
    }
}
