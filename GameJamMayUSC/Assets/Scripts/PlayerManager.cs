using UnityEngine;
using Mirror;
using TMPro;

public class PlayerManager : NetworkBehaviour
{
    public ClientManager clientManager = null;
    [HideInInspector] public PlayerController playerController;
    [SyncVar(hook = nameof(SetPlayerName))] public string playerName;
    [SyncVar(hook = nameof(SetPlayerSkin))] public int skin;
    [SerializeField] private GameObject playerNamePrefab;
    private TMP_Text playerNameText = null;
    [SerializeField] private Vector3 playerNameOffset;
    [SerializeField] private MeshRenderer playerRenderer;
    [SerializeField] private AudioSource pickupSource;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();

        if (hasAuthority)
        {
            foreach (NetworkIdentity networkIdentity in NetworkIdentity.spawned.Values)
                if (networkIdentity.isLocalPlayer)
                    clientManager = networkIdentity.GetComponent<ClientManager>();
            if (clientManager != null)
                clientManager.InitPlayer(gameObject);
        }
    }

    private void SetPlayerName(string oldName, string newName)
    {
        if (playerNameText == null)
            playerNameText = Instantiate(playerNamePrefab, transform.position, Quaternion.identity).GetComponent<TMP_Text>();
        playerNameText.text = newName;
    }

    private void SetPlayerSkin(int oldSkin, int newSkin)
    {
        playerRenderer.sharedMaterial = Skins.Instance.skins[newSkin];
    }

    private void OnDestroy()
    {
        if (playerNameText != null)
            Destroy(playerNameText.gameObject);
    }

    private void LateUpdate()
    {
        if (playerNameText == null) return;

        Transform billboardTransform = transform;
        if (SceneCamera.Instance != null && SceneCamera.Instance.gameObject.activeInHierarchy)
            billboardTransform = SceneCamera.Instance.transform;
        else if (CameraController.Instance != null && CameraController.Instance.gameObject.activeInHierarchy)
            billboardTransform = CameraController.Instance.transform;

        Transform textTransform = playerNameText.transform;
        textTransform.position = transform.position + playerNameOffset;
        textTransform.rotation = billboardTransform.rotation;
    }

    [ClientRpc]
    public void RpcPlayPickupAudio()
    {
        pickupSource.pitch = Random.Range(1f, 1.2f);
        pickupSource.PlayOneShot(pickupSource.clip);
    }
}