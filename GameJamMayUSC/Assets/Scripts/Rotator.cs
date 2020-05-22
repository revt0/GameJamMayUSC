using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script for rotating the current object
public class Rotator : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);

    }
}
