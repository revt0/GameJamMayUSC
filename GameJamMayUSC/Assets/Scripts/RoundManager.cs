using UnityEngine;
using Mirror;

public class RoundManager : NetworkBehaviour
{
    public static RoundManager Instance { get; set; }
    [SerializeField] private GameObject[] maps;
    [HideInInspector] public int currentMapIndex;
    [HideInInspector] public GameObject currentMap;
    [SerializeField] private Vector3[] spawnPoints;

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