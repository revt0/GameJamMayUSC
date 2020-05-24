using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings Instance { get; set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(Instance.gameObject);
            Instance = this;
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

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