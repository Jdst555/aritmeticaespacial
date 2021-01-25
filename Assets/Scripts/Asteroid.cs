using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour

{
    public GameObject particleSystemObject;
    public GameObject fragments;
    private float speed = 0.5f;
    private float angular;
    Vector2 dir;
    private Rigidbody2D rb;
    private Transform asteroidChild;
    private int hitpoints = 100;
    private AudioSource audioSource;
    private bool isDestroyed = false;
    

    private void Awake()
    {


        asteroidChild = transform.Find("AsteroidChild");
        audioSource = GetComponent<AudioSource>();
        rb = asteroidChild.gameObject.GetComponent<Rigidbody2D>();
        speed = Random.Range(0.2f, 0.5f);
        angular = Random.Range(-0.1f, 0.1f);
        dir = Random.insideUnitCircle.normalized;
        dir = dir * speed;
    }
    


    // Update is called once per frame
    void Update()
    {
        
        transform.Translate(new Vector3(dir.x, dir.y, 0) * Time.deltaTime);
        rb.AddTorque(angular);

        if (hitpoints <= 0 && !isDestroyed)
        {
            isDestroyed = true;
            DestroyAsteroid();
            
        }
        
    }

    private void DestroyAsteroid()
    {
        GameObject temp = particleSystemObject;
        Instantiate(temp, transform.position, Quaternion.identity);
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        GetComponent<PolygonCollider2D>().enabled = false;
        audioSource.Play();
        Destroy(this.gameObject, 3);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "Player")
        {
            Instantiate(fragments, collision.transform.position, Quaternion.identity);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Projectile")
        {
            Transform collisionTransform = collision.transform;
            Destroy(collision.gameObject);
            Instantiate(fragments, collisionTransform.position, Quaternion.identity);
            hitpoints -= 20;
        }
    }

}
