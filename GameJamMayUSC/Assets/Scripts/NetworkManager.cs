using Mirror;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : Mirror.NetworkManager
{
    [SerializeField] private bool autoStartServer;
    [SerializeField] private bool autoStartClient;
    private bool hasSelected;

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
        RoundManager.Instance.ClientLoadMap(conn);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        SceneManager.LoadScene(0, LoadSceneMode.Single);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (hasSelected) return;

        if (isHeadless || autoStartServer)
            StartCoroutine(Connect(isServer: true));
        if (autoStartClient)
            StartCoroutine(Connect(isServer: false));

        if (Input.GetKeyDown(KeyCode.I))
        {
            NetworkManager.singleton.networkAddress = "localhost";
            StartCoroutine(Connect(isServer: true));
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            NetworkManager.singleton.networkAddress = "localhost";
            StartCoroutine(Connect(isServer: false));
        }
    }

    private IEnumerator Connect(bool isServer)
    {
        hasSelected = true;
        AsyncOperation asyncLoadGame = SceneManager.LoadSceneAsync("NetworkLayer", LoadSceneMode.Single);
        while (!asyncLoadGame.isDone)
            yield return null;
        if (isServer)
            NetworkManager.singleton.StartServer();
        else
            NetworkManager.singleton.StartClient();
    }
}