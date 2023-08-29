using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public GameObject arms;
    public GameObject gunPoint;
    public GameObject torso;
    public GameObject groundCheckStart;
    public GameObject groundCheckEnd;

    public GameObject gun;
    GunGenerator generatorScript;

    private Color32 glowColor;

    // MOVEMENT
    public float runMaxSpeed;
    private float horizontal;
    public float jumpForce;
    public float boostForce;
    public float dashForce;
    public float lastOnGroundTime;
    public bool boostAvailable = true;
    public float dashCooldown;
    public float dashReady;
    [Range(0.01f, 0.5f)]  public float coyoteTime;
    public float runAcceleration;
    public float runDeceleration;
    [HideInInspector] public float runAccelAmount;
    [HideInInspector] public float runDecelAmount;
    [Range(0f, 1)] public float accelInAir;
    [Range(0f, 1)] public float deccelInAir;

    public int magSize;
    private bool automatic;
    private int damage;
    private float fireRate;
    private float nextFire;
    public float accuracy;
    public float reloadTime;

    //public float fallSpeed;

    public int currentAmmo;
    public bool isReloading;

    [HideInInspector]
    public bool isGrounded;
    private bool isBackwards;

    public Text gun_name;
    public Text rpm;
    public Text reload_time;
    public Text spread;

    Rigidbody2D rb;
    Animator anim;
    BoxCollider2D col;

    [SerializeField]
    public LayerMask groundMask;

    public float fallMultiplier;
    public float lowJumpMulitplier;

    public AudioSource step;

    void Start()
    {
        runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
        runDecelAmount = (50 * runDeceleration) / runMaxSpeed;

        generatorScript = GameObject.Find("GunGenerator").GetComponent<GunGenerator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        col = gameObject.GetComponent<BoxCollider2D>();
        gun = generatorScript.GenerateGun();
        AssignGun(gun);
    }

    void Update()
    {
        lastOnGroundTime -= Time.deltaTime;

        if(lastOnGroundTime > 0)
        {
            anim.SetBool("isGrounded", true);
            if (!((Mathf.Abs(rb.velocity.x) < 0.1)))
            {
                anim.SetBool("isWalking", true);
                if (transform.localScale.x * horizontal == 1)
                    anim.SetBool("isBackwards", false);
                else
                    anim.SetBool("isBackwards", true);
            }
            else
            {
                anim.SetBool("isWalking", false);
                anim.SetBool("isBackwards", false);
            }
            //rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
        else
        {
            anim.SetBool("isGrounded", false);
            anim.SetBool("isWalking", false);
            //rb.velocity = new Vector2(horizontal * speed / 2, rb.velocity.y);
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && (!Input.GetKey(KeyCode.Space) || !boostAvailable))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMulitplier - 1) * Time.deltaTime;
        }

        //rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -fallSpeed, fallSpeed));

        HandleInput();

        if ((Physics2D.OverlapCircle(groundCheckStart.transform.position, 0.05f, groundMask) || Physics2D.OverlapCircle(groundCheckEnd.transform.position, 0.05f, groundMask)) && rb.velocity.y == 0f)
        {
            lastOnGroundTime = coyoteTime;
            boostAvailable = true;
        }

        gun.transform.Find("Main_part").transform.Find("glow").GetComponent<SpriteRenderer>().color = new Color32(
            (byte)(glowColor.r + (255 - glowColor.r) * (magSize - currentAmmo) / (4 * magSize)),
            (byte)(glowColor.g + (255 - glowColor.g) * (magSize - currentAmmo) / (4 * magSize)),
            (byte)(glowColor.b + (255 - glowColor.b) * (magSize - currentAmmo) / (4 * magSize)),
            255
            );
    }

    void FixedUpdate()
    {
        Run();
    }

    void HandleInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Fire1") && !isReloading)
        {
            if(nextFire < Time.time)
            {
                Fire();
                nextFire = Time.time + fireRate;
                currentAmmo--;
            }
        }

        if (Input.GetButton("Fire1") && !isReloading && automatic)
        {
            if (nextFire < Time.time)
            {
                Fire();
                nextFire = Time.time + fireRate;
                currentAmmo--;
            }
        }

        //if (Input.GetKeyDown(KeyCode.Space) && lastOnGroundTime > 0)
        //{
        //    lastOnGroundTime = 0;
        //    rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        //}

        //if (Input.GetKeyDown(KeyCode.Space) && )
        //{
        //    boostAvailable = false;
        //    rb.AddForce(new Vector2(0f, boostForce), ForceMode2D.Impulse);
        //}

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (lastOnGroundTime > 0)
            {
                lastOnGroundTime = 0;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            }
            else if (boostAvailable)
            {
                boostAvailable = false;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(0f, boostForce), ForceMode2D.Impulse);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashReady < Time.time)
        {
            rb.AddForce(new Vector2(dashForce * transform.localScale.x, 0f), ForceMode2D.Impulse);
            dashReady = Time.time + dashCooldown;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject newGun = generatorScript.GenerateGun();
            AssignGun(newGun);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            var graph = AstarPath.active.data.gridGraph;
            AstarPath.active.Scan(graph);
            //graph.Scan();
        }
    }
    void Run()
    {
        float targetSpeed = horizontal * runMaxSpeed;
        targetSpeed = Mathf.Lerp(rb.velocity.x, targetSpeed, 1);

        float accelRate;
        if (lastOnGroundTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount : runDecelAmount;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount * accelInAir : runDecelAmount * deccelInAir;

        if (Mathf.Abs(rb.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(rb.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && lastOnGroundTime < 0)
            accelRate = 0;

        float speedDif = targetSpeed - rb.velocity.x;

        float movement = speedDif * accelRate;

        rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    void Fire()
    {
        gun.GetComponent<Gun>().Fire(damage, accuracy, transform.localScale.x);
        gun.GetComponent<AudioSource>().Play();
    }

    public void AssignGun(GameObject gun)
    {
        gun.transform.parent = arms.transform;
        gun.transform.position = gunPoint.transform.position;
        gun.transform.rotation = arms.transform.rotation;
        gun.transform.localScale = new Vector3(transform.localScale.x * transform.localScale.x, gun.transform.localScale.y, gun.transform.localScale.z);
        magSize = gun.GetComponent<Gun>().MagSize;
        currentAmmo = gun.GetComponent<Gun>().MagSize;
        automatic = gun.GetComponent<Gun>().Automatic;
        damage = gun.GetComponent<Gun>().Damage;
        fireRate = gun.GetComponent<Gun>().FireRate + gun.GetComponent<Gun>().FireRate * gun.GetComponent<Gun>().fireRateCounter * 0.16f;
        accuracy = gun.GetComponent<Gun>().Accuracy + gun.GetComponent<Gun>().Accuracy * gun.GetComponent<Gun>().accuracyCounter * 0.16f;
        reloadTime = gun.GetComponent<Gun>().ReloadTime + gun.GetComponent<Gun>().ReloadTime * gun.GetComponent<Gun>().reloadTimeCounter * 0.16f;
        glowColor = gun.transform.Find("Main_part").transform.Find("glow").GetComponent<SpriteRenderer>().color;
        torso.GetComponent<AudioSource>().pitch = 1;
        torso.GetComponent<AudioSource>().pitch += torso.GetComponent<AudioSource>().pitch * gun.GetComponent<Gun>().reloadTimeCounter * -0.16f;
        this.gun = gun;
        gun_name.text = "Weapon: " + gun.GetComponent<Gun>().GunName;
        rpm.text = "RPM: " + (60 / fireRate).ToString("#");
        reload_time.text = "Reload time: " + reloadTime + "s";
        spread.text = "Spread: " + accuracy + "°";
    }

    public void Step()
    {
        step.Play();
    }
}