using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseCanvas : MonoBehaviour
{
    public Canvas canvas;
    public bool isActive;


    public Slider sensXSlider;
    public Slider sensYSlider;


    private void Start()
    {
        SetActive(false);

        sensXSlider.SetValueWithoutNotify(GameManager.instance.playerCamera.xSensitivity);
        sensYSlider.SetValueWithoutNotify(GameManager.instance.playerCamera.ySensitivity);
    }

    public void SetActive(bool isActive)
    {
        this.isActive = isActive;
        canvas.enabled = isActive;
        if (!isActive)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
            GameManager.instance.playerCamera.UnlockMouse();
        }
    }

    public void OnResume()
    {
        SetActive(false);
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void OnSensXSlider(float value)
    {
        GameManager.instance.playerCamera.xSensitivity = value;
    }
    public void OnSensYSlider(float value)
    {
        GameManager.instance.playerCamera.ySensitivity = value;
    }
}
