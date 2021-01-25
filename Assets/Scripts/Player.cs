using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float linearForce = 10.0f;
    public float moment = 10.0f;
    public float inertia = 1;
    public GameObject playerExplosion;
    public float maxAngularVelocity = 400f;
    public float maxLinearVelocity = 1f;
    public GameObject shot;
    public Transform shotOrigin;
    public bool isAlive = false;
    public GameObject fragments;
    public int score;
    public Transform capturedDigitTransform;
    public AudioSource[] audioSources;
    private AudioSource jetAudioSource;
    private AudioSource shootAudioSource;
    public AudioClip goodDigitSound;
    public AudioClip badDigitSound;
    private float hMov;
    private float vMov;
    private Rigidbody2D rb;
    private GameObject part_jet_flare;
    private GameObject part_jet_core;
    private ParticleSystem flare_ps;
    private ParticleSystem core_ps;
    private GameManager gameManager;
    private float volumeInterpolation = 0;
    private float volumeSpeed = 1.2f;
    private float pitchInterpolation = 0.5f;
    private float minVolumen = 0.5f;
    private float minPitch = 0.3f;
    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void Start()
    {
        jetAudioSource = audioSources[0];
        shootAudioSource = audioSources[1];

        score = 0;
        rb = this.GetComponent<Rigidbody2D>();
        rb.inertia = inertia;
        part_jet_flare = GameObject.Find("part_jet_flare");
        part_jet_core = GameObject.Find("part_jet_core");
        flare_ps = part_jet_flare.GetComponent<ParticleSystem>();
        core_ps = part_jet_core.GetComponent<ParticleSystem>();
        isAlive = true;
        jetAudioSource.volume = minVolumen;
        jetAudioSource.pitch = minPitch;
        
    }

    // Update is called once per frame
    void Update()
    {
        //Verificar salud del jugador
        if (GetComponent<Health>().curHealth <= 0 && isAlive)
        {
            DestroyPlayer();
            if (OnPlayerDie != null)
                OnPlayerDie();
            //cambiar estado de player
            isAlive = false;
        }
        //obtener input del jugador
        if (isAlive)
        {
            hMov = Input.GetAxisRaw("Horizontal");
            vMov = Mathf.Clamp(Input.GetAxisRaw("Vertical"), 0, 1);
            //instanciar disparo si se preciona shift
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                
                GameObject instantiatedShot = Instantiate(shot, shotOrigin.position, shotOrigin.rotation);
                instantiatedShot.layer = 8;
                instantiatedShot.GetComponent<MoveShot>().speed = 20;
                instantiatedShot.GetComponent<MoveShot>().shootingObjectTransform = transform;
                shootAudioSource.Play();
            }
            //controlar visual motor jet
            var flare_em = flare_ps.emission;
            var core_em = core_ps.emission;
            flare_em.rateOverTime = vMov * 15;
            core_em.rateOverTime = vMov * 15;
        }
        JetSound();
        
    }

    void FixedUpdate()
    {
        Move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        //que hacer cuando choca con un asteroide
        if (collision.gameObject.tag == "Asteroid")
        {
            GetComponent<Health>().DamagePlayer((int)(collision.relativeVelocity.magnitude * 5));
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //que hacer cuando captura un digito
        if (collision.tag == "Digit")
        {
            //Debug.Log("PLAYER/TRIGGER_ENTER digit is: " + collision.GetComponent<Digit>().GetDigit());
            if (collision.GetComponent<Digit>().GetDigit() == gameManager.GetCurrentDigitToFind())
            {
                if (OnGoodDigitFound != null)
                {
                    capturedDigitTransform = collision.transform;
                    OnGoodDigitFound();
                    audioSources[2].clip = goodDigitSound;
                    audioSources[2].Play();
                    Destroy(collision.gameObject);
                }
            }
            //que hacer cuando captura un digito equivocado
            else {
                OnBadDigitFound?.Invoke();
                audioSources[2].clip = badDigitSound;
                audioSources[2].Play();
                Destroy(collision.gameObject);
                //Debug.Log("Wrong digit.");
            }
        }
        if (collision.tag == "Projectile")
        {
            GetComponent<Health>().DamagePlayer(10);
            Destroy(collision.gameObject);
            Instantiate(fragments, transform.position, Quaternion.identity);
        }
    }
    //movimiento del jugador
    private void Move()
    {
        rb.AddForce(transform.up * vMov * linearForce * Time.deltaTime);
        rb.AddTorque(-hMov * moment);
        
        if (rb.angularVelocity > maxAngularVelocity)
        {
            rb.angularVelocity = maxAngularVelocity;
        }
        else if (rb.angularVelocity < -maxAngularVelocity)
        {
            rb.angularVelocity = -maxAngularVelocity;
        }

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxLinearVelocity);
    }
    private void JetSound()
    {

        if (Input.GetKey(KeyCode.UpArrow))
        {
            volumeInterpolation = Mathf.Clamp01(volumeInterpolation += Time.deltaTime * volumeSpeed);
            pitchInterpolation = Mathf.Clamp01( pitchInterpolation += Time.deltaTime * volumeSpeed);
            jetAudioSource.volume = Mathf.Lerp(minVolumen, 1f, volumeInterpolation);
            jetAudioSource.pitch = Mathf.Lerp(minPitch, 1.5f, pitchInterpolation);
        }
        else
        {
            volumeInterpolation = Mathf.Clamp01(volumeInterpolation -= Time.deltaTime * volumeSpeed);
            pitchInterpolation = Mathf.Clamp01(pitchInterpolation -= Time.deltaTime * volumeSpeed);
            jetAudioSource.volume = Mathf.Lerp(minVolumen, 1f, volumeInterpolation);
            jetAudioSource.pitch = Mathf.Lerp(minPitch, 1.5f, pitchInterpolation);
        }
    }
    //que hacer cuando el jugador es destruido
    public void DestroyPlayer()
    {
        StartCoroutine("playerDestructionSequence");
    }

    //corutina de destruccion del jugador. (el gameObject "player" NO es destruido, solo desabilitado los elementos graficos)
    private IEnumerator playerDestructionSequence()
    {
        //instanciar explosion
        Instantiate(playerExplosion, transform.position, Quaternion.identity);
        //desactivar los misiles
        transform.Find("Missiles").gameObject.SetActive(false);
        //desactivar motor jet
        transform.Find("engines_player").gameObject.SetActive(false);
        //desactivar el renderer de player
        GetComponent<SpriteRenderer>().enabled = false;
        //obtener la duracion de la explosion de player
        float time = playerExplosion.GetComponent<ParticleSystem>().main.duration;
        //esperar que termine la explosion
        yield return new WaitForSeconds(time);
        //detener tiempo
        Time.timeScale = 0;
    }

    public delegate void PlayerDies();
    public static event PlayerDies OnPlayerDie;

    public delegate void GoodDigitFound();
    public static event GoodDigitFound OnGoodDigitFound;

    public delegate void BadDigitFound();
    public static event BadDigitFound OnBadDigitFound;
}
