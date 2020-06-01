using UnityEngine;
using Mirror;

public class SceneCamera : MonoBehaviour
{
    public static SceneCamera Instance { get; set; }
    [SerializeField] private GameObject directionalLight = null;

    private void Awake()
    {
        if (CameraController.Instance != null && CameraController.Instance.gameObject.activeInHierarchy)
            gameObject.SetActive(false);

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