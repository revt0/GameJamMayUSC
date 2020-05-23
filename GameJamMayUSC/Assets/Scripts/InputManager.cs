using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputPacket packet = new InputPacket();

    private void Update()
    {
        packet.horizontal = (sbyte)Input.GetAxisRaw("Horizontal");
        packet.vertical = (sbyte)Input.GetAxisRaw("Vertical");
    }
}

public struct InputPacket
{
    public sbyte horizontal;
    public sbyte vertical;

    public InputPacket(sbyte horizontal, sbyte vertical)
    {
        this.horizontal = horizontal;
        this.vertical = vertical;
    }
}