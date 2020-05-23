using UnityEngine;
using Mirror;

public class RoundManager : NetworkBehaviour
{
    public static RoundManager Instance { get; set; }
    [SerializeField] private GameObject[] maps;
    [HideInInspector] public int currentMapIndex;
    [HideInInspector] public GameObject currentMap;
    [SerializeField] private Vector3[] spawnPoints;
    [SerializeField] private Transform[] pickupSpawns;
    [SerializeField] private GameObject pickupPrefab;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        if (NetworkServer.active)
            InitServer();
    }

    public void InitServer()
    {
        LoadMap();
    }

    public void LoadMap(int mapIndex = -1)
    {
        if (mapIndex == -1) mapIndex = Random.Range(0, maps.Length);
        print($"Load Map: {mapIndex}");
        currentMap = Instantiate(maps[mapIndex], Vector3.zero, Quaternion.identity);
        currentMapIndex = mapIndex;
        if (NetworkServer.active)
            SpawnPickups();
    }

    private void SpawnPickups()
    {
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
        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }
}