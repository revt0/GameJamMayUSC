using UnityEngine;

public class Skybox : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    private void LateUpdate()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeed);
    }
}