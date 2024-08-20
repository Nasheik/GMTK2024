using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] Transform playerSpawn;
    [HideInInspector] public Player player;


    public Input input;
    public MoveCamera moveCamera;
    public PlayerCamera playerCamera;

    public PauseCanvas pauseCanvas;


    private void Awake()
    {
        Application.targetFrameRate = 60;
        if(!instance) instance = this;
        else Destroy(this);
        Debug.unityLogger.logEnabled = false;
    }

    public void ResetPlayer()
    {
        player.ResetPlayerPosition(playerSpawn.position);
    }
    public void FullResetPlayer()
    {
        player.goldFlags[0] = false;
        player.goldFlags[1] = false;
        player.goldFlags[2] = false;
        player.checkpoint = null;
        player.ResetPlayerPosition(playerSpawn.position);
    }
}
