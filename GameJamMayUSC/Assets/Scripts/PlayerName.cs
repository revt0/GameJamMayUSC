using TMPro;
using UnityEngine;

public class PlayerName : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInput;

    private void Awake()
    {
        SetName();
    }

    public void SetName()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
            nameInput.text = PlayerPrefs.GetString("PlayerName");
    }

    public void SaveName(string name)
    {
        PlayerPrefs.SetString("PlayerName", name);
    }
}