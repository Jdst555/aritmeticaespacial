using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DigitsLimit : MonoBehaviour
{

    private GameObject rightLimit, leftLimit, topLimit, bottomLimit;
    private float displacement = 50;

    private void Awake()
    {
        rightLimit = GameObject.Find("Right limit");
        leftLimit = GameObject.Find("Left limit");
        topLimit = GameObject.Find("Top limit");
        bottomLimit = GameObject.Find("Bottom limit");



    }
    private void OnTriggerEnter2D (Collider2D other)
    {
        
        if (other.tag == "Digit")
        {
            if (this.name == "Right limit")
            {
                other.transform.position += new Vector3(-displacement, 0, 0);
            }
            else if (this.name == "Left limit")
            {
                other.transform.position += new Vector3(displacement, 0, 0);
            }
            else if (this.name == "Top limit")
            {
                other.transform.position += new Vector3(0, -displacement, 0);
            }
            else if (this.name == "Bottom limit")
            {
                other.transform.position += new Vector3(0, displacement, 0);
            }
        }
    }
}

    