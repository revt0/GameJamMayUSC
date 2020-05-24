using UnityEngine;
using Mirror;
using TMPro;

public class ClientManager : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerCam;
    [SerializeField] private GameObject playerCanvas;
    [SerializeField] private GameObject skinSelectGO;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private Vector3 skinSelectPosOffset;
    [SerializeField] private Vector3 skinSelectRotOffset;
    [SerializeField] private SkinSelect skinSelect;
    [SerializeField] private AudioSource joinSource;
    private bool hasSpawned;
    private GameObject spawnedPlayer;
    private PlayerManager playerManager;
    private int ballNameCounter;
    public Vector3 spawnPos;

    private void Start()
    {
        if (isLocalPlayer)
            InitLocalClient();
    }

    private void InitLocalClient()
    {
        playerCanvas.SetActive(true);
        if (SceneCamera.Instance != null)
        {
            skinSelectGO.transform.SetParent(SceneCamera.Instance.transform);
            skinSelectGO.transform.localPosition = skinSelectPosOffset;
            skinSelectGO.transform.localRotation = Quaternion.Euler(skinSelectRotOffset);
        }
        skinSelectGO.SetActive(true);
    }

    public void SpawnRequest()
    {
        if (isLocalPlayer)
        {
            CmdSpawnRequest(nameInput.text, skinSelect.currentSkin);
            PlayerPrefs.SetInt("PlayerSkin", skinSelect.currentSkin);
            joinSource.PlayOneShot(joinSource.clip);
        }
    }

    [Command]
    private void CmdSpawnRequest(string playerName, int skin)
    {
        if (!hasSpawned)
            ServerSpawn(playerName, skin);
    }

    private void ServerSpawn(string playerName, int skin)
    {
        hasSpawned = true;
        RoundManager.Instance.clients.Add(this);
        spawnPos = RoundManager.Instance.GetSpawnPoint();
        spawnedPlayer = Instantiate(playerPrefab, spawnPos, Quaternion.identity, transform);
        playerManager = spawnedPlayer.GetComponent<PlayerManager>();
        if (playerName.Trim() == "")
        {
            ballNameCounter++;
            playerName = $"Ball {ballNameCounter}";
        }
        playerManager.playerName = playerName;
        playerManager.clientManager = this;
        playerManager.skin = skin;
        NetworkServer.Spawn(spawnedPlayer, gameObject);
        inputHandler.playerController = playerManager.GetComponent<PlayerController>();
    }

    public void InitPlayer(GameObject player)
    {
        if (SceneCamera.Instance != null)
            SceneCamera.Instance.gameObject.SetActive(false);
        playerCanvas.SetActive(false);
        skinSelectGO.SetActive(false);
        skinSelectGO.transform.SetParent(transform);
        RoundManager.Instance.roundCanvas.enabled = true;
        cameraController.player = player;
        playerCam.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDestroy()
    {
        if (NetworkServer.active && RoundManager.Instance.clients.Contains(this))
            RoundManager.Instance.clients.Remove(this);
    }

    public void Respawn()
    {
        if (playerManager != null)
            playerManager.playerController.Respawn();
    }
}