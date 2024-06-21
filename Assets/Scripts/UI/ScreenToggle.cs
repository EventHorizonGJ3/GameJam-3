using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScreenToggle : MonoBehaviour
{
    Toggle fullscreenToggle;

    private void Awake()
    {
        fullscreenToggle = GetComponent<Toggle>();
    }

    void Start()
    {

        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        Screen.fullScreen = true;


        fullscreenToggle.isOn = Screen.fullScreen;


        fullscreenToggle.onValueChanged.AddListener(delegate
        {
            ToggleFullScreenMode(fullscreenToggle.isOn);
        });
    }


    public void ToggleFullScreenMode(bool isFullscreen)
    {
        if (isFullscreen)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            Screen.fullScreen = false;
        }
    }
}
