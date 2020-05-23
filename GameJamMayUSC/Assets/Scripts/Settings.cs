using UnityEngine;

public class Settings : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 300;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Fullscreen();
    }

    private void Fullscreen()
    {
        if (Screen.fullScreenMode == FullScreenMode.Windowed)
        {
            Resolution monitorRes = Screen.resolutions[Screen.resolutions.Length - 1];
            Screen.SetResolution(monitorRes.width, monitorRes.height, FullScreenMode.FullScreenWindow);
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        else
        {
            Screen.SetResolution(1280, 720, false);
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }
}