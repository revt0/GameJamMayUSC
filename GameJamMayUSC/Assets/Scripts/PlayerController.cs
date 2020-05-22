using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//written by Andrew Denman
public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;

    private int winNum;
    private int count;
    //public Text countText;
    //public Text winText;

    private void Start()
    {
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
        
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);
    }
    //Collision with pick up objects
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count++;
            //setCountText();
        }
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
