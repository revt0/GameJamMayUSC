using UnityEngine;
using Mirror;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class RoundManager : NetworkBehaviour
{
    public static RoundManager Instance { get; set; }
    [SerializeField] private GameObject[] maps;
    [HideInInspector] public int currentMapIndex;
    [HideInInspector] public GameObject currentMap;
    [SerializeField] private Vector3[] spawnPoints;
    [SerializeField] private Transform[] pickupSpawns;
    [SerializeField] private GameObject pickupPrefab;
    public List<ClientManager> clients = new List<ClientManager>();
    [SyncVar(hook = nameof(SetRoundInfo))] public string roundInfo;
    public Canvas roundCanvas;
    [SerializeField] private TMP_Text roundText;
    private List<GameObject> pickups = new List<GameObject>();

    public enum RoundState { Waiting, Active, Finished};
    public RoundState roundState = RoundState.Waiting;
    private float countdownTimer;
    [HideInInspector] public bool stopPlayerInput;
    private float roundTimer;
    private float winTimer;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        if (NetworkServer.active)
        {
            InitServer();
        }
    }

    private void Update()
    {
        if (!NetworkServer.active) return;

        if (clients.Count > 1 && roundState == RoundState.Waiting)
            StartRound();
        else if (roundState == RoundState.Waiting && roundInfo != "Waiting for Players...")
            roundInfo = "Waiting for Players...";

        if (clients.Count <= 0 && roundState == RoundState.Active)
        {
            roundState = RoundState.Waiting;
            roundInfo = "Waiting for Players...";
        }

        if (countdownTimer > 0f && roundState == RoundState.Active)
        {
            countdownTimer = Mathf.Max(0f, countdownTimer - Time.deltaTime);
            roundInfo = Mathf.Ceil(countdownTimer).ToString();
            if (countdownTimer == 0f)
            {
                roundInfo = "Go!";
                stopPlayerInput = false;
                Invoke("ClearInfo", 1.5f);
            }
        }
        else if (roundState == RoundState.Active)
        {
            roundTimer += Time.deltaTime;
            if (roundInfo != "Go!")
                roundInfo = roundTimer.ToString("F1");
        }
        else if (roundState == RoundState.Finished)
        {
            winTimer = Mathf.Max(0f, winTimer - Time.deltaTime);
            if (winTimer == 0f)
            {
                StartCoroutine(DisconnectAll());
                //roundState = RoundState.Waiting;
            }
        }
    }

    private void ClearInfo()
    {
        roundInfo = "";
    }

    public void InitServer()
    {
        Application.targetFrameRate = 60;
        LoadMap();
    }

    private void SetRoundInfo(string oldInfo, string newInfo)
    {
        if (oldInfo == newInfo) return;
        roundText.text = newInfo;
    }

    public void LoadMap(int mapIndex = -1)
    {
        if (mapIndex == -1) mapIndex = Random.Range(0, maps.Length);
        currentMap = Instantiate(maps[mapIndex], Vector3.zero, Quaternion.identity);
        currentMapIndex = mapIndex;
        if (NetworkServer.active)
        {
            SpawnPoints();
            SpawnPickups();
        }
    }

    private void StartRound()
    {
        stopPlayerInput = true;
        foreach (ClientManager clientManager in clients)
            clientManager.Respawn();
        SpawnPickups();
        countdownTimer = 5f;
        roundTimer = 0f;
        roundState = RoundState.Active;
    }

    private IEnumerator DisconnectAll()
    {
        yield return new WaitForSeconds(6f);
        NetworkServer.DisconnectAllConnections();
        NetworkManager.singleton.StopServer();
        while (NetworkManager.singleton.isNetworkActive)
            yield return null;
        Destroy(NetworkManager.singleton.gameObject);
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void PlayerFinished(PlayerManager playerManager)
    {
        if (roundState != RoundState.Active) return;

        roundState = RoundState.Finished;
        roundInfo = $"{playerManager.playerName} Wins!\n({roundTimer:F1} seconds)";
        winTimer = 10f;
    }

    private void SpawnPoints()
    {
        GameObject[] spawnPointGOs = GameObject.FindGameObjectsWithTag("Spawn Point");
        spawnPoints = new Vector3[spawnPointGOs.Length];
        for (int i = 0; i < spawnPointGOs.Length; i++)
            spawnPoints[i] = spawnPointGOs[i].transform.position;
        if (spawnPointGOs.Length == 0)
        {
            spawnPoints = new Vector3[30];
            for (int i = 0; i < spawnPoints.Length; i++)
                spawnPoints[i] = new Vector3(0f, 0.5f, 0f);
        }
    }

    private void SpawnPickups()
    {
        foreach (GameObject go in pickups)
            NetworkServer.Destroy(go);
        GameObject[] pickupSpawnsGOs = GameObject.FindGameObjectsWithTag("Pickup Spawn");
        pickupSpawns = new Transform[pickupSpawnsGOs.Length];
        for (int i = 0; i < pickupSpawnsGOs.Length; i++)
            pickupSpawns[i] = pickupSpawnsGOs[i].transform;
        pickupSpawns = ShuffleTransforms(pickupSpawns);
        int startIndex = (int)(pickupSpawns.Length * 0.3f);
        int endIndex = (int)(pickupSpawns.Length * 0.8f);
        for (int i = startIndex; i < endIndex; i++)
        {
            GameObject pickupGO = Instantiate(pickupPrefab, pickupSpawns[i].position, Quaternion.identity);
            NetworkServer.Spawn(pickupGO);
            pickups.Add(pickupGO);
        }    
    }

    private Transform[] ShuffleTransforms(Transform[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            int rnd = Random.Range(0, arr.Length);
            Transform tmp = arr[rnd];
            arr[rnd] = arr[i];
            arr[i] = tmp;
        }

        return arr;
    }

    public void ClientLoadMap(NetworkConnection conn)
    {
        TargetLoadMap(conn, currentMapIndex);
    }

    [TargetRpc]
    private void TargetLoadMap(NetworkConnection target, int mapIndex)
    {
        print("Load Map from Server");
        LoadMap(mapIndex);
    }

    public Vector3 GetSpawnPoint()
    {
        return spawnPoints[clients.Count - 1];
    }
}