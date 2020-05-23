using Mirror;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : Mirror.NetworkManager
{
    [SerializeField] private bool autoStartClient;
    private bool hasSelected;

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
        RoundManager.Instance.ClientLoadMap(conn);
    }

    private void Update()
    {
        if (hasSelected) return;

        if (isHeadless)
            StartCoroutine(Connect(isServer: true));
        if (autoStartClient)
            StartCoroutine(Connect(isServer: false));

        if (Input.GetKeyDown(KeyCode.I))
            StartCoroutine(Connect(isServer: true));
        else if (Input.GetKeyDown(KeyCode.L))
            StartCoroutine(Connect(isServer: false));
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