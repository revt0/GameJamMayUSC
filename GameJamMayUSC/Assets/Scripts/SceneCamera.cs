using UnityEngine;

public class SceneCamera : MonoBehaviour
{
    public static SceneCamera Instance { get; set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}