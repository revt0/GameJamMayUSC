using UnityEngine;

//written by Andrew Denman
public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;

    public void ApplyMovement(InputPacket packet)
    {
        Vector3 movement = new Vector3(packet.horizontal, 0.0f, packet.vertical);
        rb.AddForce(movement * speed);
    }

    //Collision with pick up objects
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
        }
    }
}