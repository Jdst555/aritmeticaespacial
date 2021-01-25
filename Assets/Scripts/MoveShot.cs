using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveShot : MonoBehaviour
{
    public float speed = 15;
    private Rigidbody2D rb;
    public Transform shootingObjectTransform;
    
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (shootingObjectTransform != null)
        {
            rb.velocity = shootingObjectTransform.GetComponent<Rigidbody2D>().velocity + (Vector2)(transform.up * speed);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Asteroid")
        {
            Destroy(this.gameObject);
        }
        
    }

    private void DestroyProjectile() { }
}
