using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour

{
    public GameObject fragments;
    public GameObject shot;
    public Transform shotOrigin;
    public GameObject explosion;
    public bool isShooting = true;
    public float enemyLifeTime = 20;
    private GameObject player;
    public int curHealth = 0;
    public int maxHealth = 100;
    public int damage = 20;
    public float moment = 50;
    public float inertia = 1;
    public float chaseSpeed = 1;
    public float smoothTime = 0.5f;
    public float chaseDistance = 10;
    private Vector3 findPlayerVector = new Vector3();
    private Rigidbody2D rb;
    private float deltaAngle;
    private float fireRate = 1;
    private float lastTimeShot = 0;
    private int shotSpeed = 15;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        shotOrigin = transform;
        player = GameObject.FindGameObjectWithTag("Player");
        curHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        rb.inertia = inertia;
        Destroy(this.gameObject, enemyLifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            findPlayerVector = player.transform.position - transform.position;//preparar el vector de busqueda del jugador
            if (deltaAngle < 15 && findPlayerVector.magnitude < 10 && isShooting)
                Shoot(shot);
        }
        
        if (curHealth <= 0)
        {
            DestroyEnemy();
        }

        Chase();

    }
    private void FixedUpdate()
    {
        SteerEnemyToPlayer(player, moment, findPlayerVector);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Projectile")
        {
            audioSource.Play();
            Transform collisionTransform = collision.transform;
            Destroy(collision.gameObject);
            Instantiate(fragments, collisionTransform.position, Quaternion.identity);
            DamageEnemy(damage);
        }
    }
    private void Patrol()
    {

    }

    private void Chase()
    {
        if (findPlayerVector.magnitude > 5)
        {
            float referenceVelocity = 0f;
            float rigidBodyVelocity = Mathf.SmoothDamp(rb.velocity.magnitude, chaseSpeed, ref referenceVelocity, smoothTime);
            rb.velocity = transform.up * rigidBodyVelocity;
        }
        else
        {
            float referenceVelocity = 0f;
            float rigidBodyVelocity = Mathf.SmoothDamp(rb.velocity.magnitude, 0f, ref referenceVelocity, smoothTime);
            rb.velocity = transform.up * rigidBodyVelocity;
        }
       
    }


    private void Shoot(GameObject shot_)
    {
        float now = Time.time;
        if (now - lastTimeShot > fireRate)
        {
            GameObject shot = (GameObject)Instantiate(shot_, shotOrigin.position, shotOrigin.rotation);
            shot.layer = 9;
            shot.GetComponent<MoveShot>().speed = shotSpeed;
            shot.GetComponent<MoveShot>().shootingObjectTransform = transform;
            lastTimeShot = now;
        }
    }

    private void DestroyEnemy()
    {
        OnEnemyDestroyEvent?.Invoke(transform);
        Instantiate(explosion,transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    private void DamageEnemy(int damage)
    {
        curHealth -= damage;
        Debug.Log("Enemy health: " + curHealth);
    }

    private void SteerEnemyToPlayer(GameObject player, float moment, Vector3 vectorToPlayer)
    {
        vectorToPlayer = player.transform.position - transform.position;
        vectorToPlayer = vectorToPlayer.normalized;
        deltaAngle = Vector3.SignedAngle(transform.up, vectorToPlayer, Vector3.forward);
        rb.AddTorque(deltaAngle * moment);
    }

    public delegate void OnEnemyDestroy(Transform enemyTransform);
    public static event OnEnemyDestroy OnEnemyDestroyEvent;
}
