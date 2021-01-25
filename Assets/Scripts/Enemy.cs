using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour

{
    public GameObject fragments;
    public GameObject shot;
    public Transform shotOrigin;
    public GameObject explosion;
    private GameObject player;
    public int curHealth = 0;
    public int maxHealth = 100;
    public float moment = 50;
    public float inertia = 1;
    private Vector3 findPlayerVector = new Vector3();
    private Rigidbody2D rb;
    private float deltaAngle;
    private float fireRate = 1;
    private float lastTimeShot = 0;

    void Start()
    {
        shotOrigin = transform;
        player = GameObject.FindGameObjectWithTag("Player");
        curHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        rb.inertia = inertia;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            findPlayerVector = player.transform.position - transform.position;
            if (deltaAngle < 15)
                Shoot(shot);
        }
        
        if (curHealth <= 0)
        {

            DestroyEnemy();

        }

    }
    private void FixedUpdate()
    {
        SteerEnemyToPlayer(player, moment);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Projectile")
        {
            Transform collisionTransform = collision.transform;
            Destroy(collision.gameObject);
            Instantiate(fragments, collisionTransform.position, Quaternion.identity);
            DamageEnemy(5);
        }
    }
    private void Patrol()
    {

    }

    private void Chase()
    {
    }

    private void Shoot(GameObject shot_)
    {
        float now = Time.time;
        if (now - lastTimeShot > fireRate)
        {
            GameObject shot = (GameObject)Instantiate(shot_, shotOrigin.position, shotOrigin.rotation);
            shot.layer = 9;
            shot.GetComponent<MoveShot>().speed = 25;
            shot.GetComponent<MoveShot>().shootingObjectTransform = transform;
            lastTimeShot = now;
        }
    }

    private void DestroyEnemy()
    {
        Instantiate(explosion,transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    private void DamageEnemy(int damage)
    {
        curHealth -= damage;
        Debug.Log("Enemy health: " + curHealth);
    }

    private void SteerEnemyToPlayer(GameObject player, float moment)
    {
        findPlayerVector = player.transform.position - transform.position;
        findPlayerVector = findPlayerVector.normalized;
        deltaAngle = Vector3.SignedAngle(transform.up, findPlayerVector, Vector3.forward);
        rb.AddTorque(deltaAngle * moment);
    }
}
