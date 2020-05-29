using UnityEngine;
using Mirror;

public class SceneCamera : MonoBehaviour
{
    public static SceneCamera Instance { get; set; }
    [SerializeField] private GameObject directionalLight = null;

    private void Awake()
    {
        if (NetworkServer.active)
        {
            if (directionalLight != null) Destroy(directionalLight);
            Destroy(gameObject);
            return;
        }

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}