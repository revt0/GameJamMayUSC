using UnityEngine;

public class Skins : MonoBehaviour
{
    public static Skins Instance { get; set; }
    public Material[] skins;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}