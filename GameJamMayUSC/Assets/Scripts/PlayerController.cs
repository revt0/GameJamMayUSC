using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//written by Andrew Denman
public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    [SerializeField] private float speed;
    private float defaultSpeed = 10f;
    private bool sbBool;

    private float speedBoost;
    [SerializeField] private Vector3 spawnPos;

    //visual variables
    public MeshRenderer boostRend;
    private int winNum;
    private int count;
    //public Text countText;
    //public Text winText;

    private void Start()
    {
        speed = defaultSpeed;
        spawnPos = new Vector3(0.0f, 0.5f, 0.0f);
        speedBoost = 0;
        count = 0;
        //setCountText();
       //winText.text = "";
        winNum = 12;
    }
    private void Update()
    {
        
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
            count++;
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

    private void respawn ()
    {
        resetAll();
        transform.position = spawnPos;
        
    }

    private void resetAll ()
    {
        rb.Sleep();
        speed = defaultSpeed;
        count = 0;
        rb.WakeUp();
    }
    private void setCountText()
    {
        /*countText.text = "Count: " + count.ToString();
        if (count >= winNum)
        {
            winText.text = "You Win!";
        }*/
    }
}
