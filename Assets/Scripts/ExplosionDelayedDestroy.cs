using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDelayedDestroy : MonoBehaviour
{
    private float time = 3f;
    void Start()
    {
        GameObject.Destroy(this.gameObject, time);
    }

   
    
}
