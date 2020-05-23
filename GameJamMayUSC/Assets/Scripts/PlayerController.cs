using UnityEngine;
using Mirror;

//written by Andrew Denman
public class PlayerController : MonoBehaviour
{
    public bool localTest;
    public Rigidbody rb;
    public float speed;

    private float defaultSpeed = 10f;
    private bool sbBool;
    private float speedBoost;
    [SerializeField] private Vector3 spawnPos;

    //visual variables
    public MeshRenderer boostRend;


    public void ApplyMovement(InputPacket packet)
    {
        if (speedBoost >= 180)
        {
            speed = defaultSpeed;
            speedBoost = 0;
            sbBool = false;
            //boostRend.enabled = false;
        }
        else if (sbBool)
        {
            speedBoost++;
        }

        Vector3 movement = new Vector3(packet.horizontal, 0.0f, packet.vertical);
        rb.AddForce(movement * speed);
    }

    private void Start()
    {
        speed = defaultSpeed;
        spawnPos = new Vector3(0.0f, 0.5f, 0.0f);
        speedBoost = 0;
 
    }

    //Fixed input for movement
    private void FixedUpdate()
    {
        if (!localTest) return;
        //movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);
        //end movement


        //speed boost
        if (speedBoost >= 180)
        {
            speed = defaultSpeed;
            speedBoost = 0;
            sbBool = false;
            boostRend.enabled = false;
        }
        else if (sbBool)
        {
            speedBoost++;
        }
        //end speedboost
    }
    //Collision with pick up objects
    private void OnTriggerEnter(Collider other)
    {
        if (!localTest && !NetworkServer.active) return;

        //Pick up counter
        if (other.gameObject.CompareTag("Pick Up"))
        {
            NetworkServer.Destroy(other.gameObject);

            //boostRend.enabled = true;
            speed += defaultSpeed;
            sbBool = true;
            //setCountText();
        }

        //
        if (other.gameObject.CompareTag("Kill Box"))
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        ResetAll();
        transform.position = spawnPos;

    }

    private void ResetAll()
    {
        rb.Sleep();
        speed = defaultSpeed;
        rb.WakeUp();
    }
}