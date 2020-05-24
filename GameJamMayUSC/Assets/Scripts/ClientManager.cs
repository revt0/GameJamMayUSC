using UnityEngine;
using Mirror;
using TMPro;

public class ClientManager : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerCam;
    [SerializeField] private GameObject playerCanvas;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private TMP_InputField nameInput;
    private bool hasSpawned;
    private GameObject spawnedPlayer;
    private PlayerManager playerManager;

    private void Start()
    {
        if (isLocalPlayer)
            InitLocalClient();
    }

    private void InitLocalClient()
    {
        playerCanvas.SetActive(true);
    }

    public void SpawnRequest()
    {
        if (isLocalPlayer)
            CmdSpawnRequest(nameInput.text);
    }

    [Command]
    private void CmdSpawnRequest(string playerName)
    {
        if (!hasSpawned)
            ServerSpawn(playerName);
    }

    private void ServerSpawn(string playerName)
    {
        hasSpawned = true;
        spawnedPlayer = Instantiate(playerPrefab, RoundManager.Instance.GetSpawnPoint(), Quaternion.identity, transform);
        playerManager = spawnedPlayer.GetComponent<PlayerManager>();
        playerManager.playerName = playerName;
        NetworkServer.Spawn(spawnedPlayer, gameObject);
        inputHandler.playerController = playerManager.GetComponent<PlayerController>();
    }

    public void InitPlayer(GameObject player)
    {
        if (SceneCamera.Instance != null)
            SceneCamera.Instance.gameObject.SetActive(false);
        playerCanvas.SetActive(false);
        cameraController.player = player;
        playerCam.SetActive(true);
    }
}