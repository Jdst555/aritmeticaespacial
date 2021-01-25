using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Script para que la camara siga al jugador
 */
public class Follow : MonoBehaviour
{
    // Transform del jugador
    public Transform playerTransform;

    [System.NonSerialized]
    public Transform newTransform;
    [System.NonSerialized]
    public Vector3 deltaPosition = new Vector3();
    
    
    void Start()
    {
        newTransform = transform;
       
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
        {
            deltaPosition = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z) - transform.position;

            newTransform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
            transform.position = newTransform.position;
        }
        else { deltaPosition = Vector3.zero; }
        
    }
}
