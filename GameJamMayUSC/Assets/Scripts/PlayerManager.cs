using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    private ClientManager clientManager = null;
    [HideInInspector] public PlayerController playerController;

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
}