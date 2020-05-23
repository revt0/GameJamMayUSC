using UnityEngine;

public class CameraController : MonoBehaviour
{
    [HideInInspector] public GameObject player;
    private Vector3 offset;

    private void Start()
    {
        offset = transform.position - player.transform.position;
    }

    private void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}