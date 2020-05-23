﻿using UnityEngine;

//written by Andrew Denman
public class PlayerController : MonoBehaviour
{
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
        //Pick up counter
        if (other.gameObject.CompareTag("Pick Up"))
        {

            other.gameObject.SetActive(false);

            boostRend.enabled = true;
            speed += defaultSpeed;
            sbBool = true;
            //setCountText();
        }

        //
        if (other.gameObject.CompareTag("Kill Box"))
        {
            respawn();
        }
    }

    private void respawn()
    {
        resetAll();
        transform.position = spawnPos;

    }

    private void resetAll()
    {
        rb.Sleep();
        speed = defaultSpeed;
        rb.WakeUp();
    }
}