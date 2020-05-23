using UnityEngine;
using Mirror;

public class InputHandler : NetworkBehaviour
{
    [HideInInspector] public PlayerController playerController = null;

    private void FixedUpdate()
    {
        SendInput();
    }

    private void SendInput()
    {
        if (!isLocalPlayer) return;

        InputPacket packet = InputManager.packet;
        if (packet.horizontal == 0 && packet.vertical == 0) return;
        CmdSendInput(packet.horizontal, packet.vertical);
    }

    [Command]
    private void CmdSendInput(sbyte horizontal, sbyte vertical)
    {
        InputPacket packet = new InputPacket(horizontal, vertical);
        if (playerController != null) playerController.ApplyMovement(packet);
    }
}