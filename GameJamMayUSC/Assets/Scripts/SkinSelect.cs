using UnityEngine;

public class SkinSelect : MonoBehaviour
{
    [SerializeField] private MeshRenderer skinSelectRenderer;
    [HideInInspector] public int currentSkin;
    [SerializeField] private AudioSource skinButtonSource;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("PlayerSkin"))
        {
            currentSkin = PlayerPrefs.GetInt("PlayerSkin");
            skinSelectRenderer.sharedMaterial = Skins.Instance.skins[currentSkin];
        }
    }

    public void RightArrow()
    {
        currentSkin++;
        if (currentSkin >= Skins.Instance.skins.Length)
            currentSkin = 0;
        skinSelectRenderer.sharedMaterial = Skins.Instance.skins[currentSkin];
        skinButtonSource.PlayOneShot(skinButtonSource.clip);
    }

    public void LeftArrow()
    {
        currentSkin--;
        if (currentSkin < 0)
            currentSkin = Skins.Instance.skins.Length - 1;
        skinSelectRenderer.sharedMaterial = Skins.Instance.skins[currentSkin];
        skinButtonSource.PlayOneShot(skinButtonSource.clip);
    }
}