using UnityEngine;


public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; set; }
    public GameObject player;
    private Vector3 offset;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        offset = new Vector3(0f, 10f, -10f);
    }

    private void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}