using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform playerSpawn;
    [HideInInspector] public Player player;


    private void Awake()
    {
        Application.targetFrameRate = 60;
        DontDestroyOnLoad(gameObject);
    }

    public void ResetPlayer()
    {
        player.TeleportPlayer(playerSpawn.position);
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame) Application.Quit();
        if (Keyboard.current.rKey.wasPressedThisFrame) ResetPlayer();
    }
}
