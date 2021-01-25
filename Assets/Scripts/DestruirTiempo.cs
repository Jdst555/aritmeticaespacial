using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestruirTiempo : MonoBehaviour
{
    // Start is called before the first frame update
    public float time = 2;
    void Start()
    {
        GameObject.Destroy(this.gameObject, time);
    }
}
