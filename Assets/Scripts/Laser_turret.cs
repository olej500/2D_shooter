using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_turret : MonoBehaviour
{
    GameObject player;
    GameObject barrel;
    float laserLength = 20f;
    public LayerMask playerGroundMask;
    public LayerMask groundMask;
    float coolDownTime = 3f;
    float shootTime = 0;
    float laserChargeTime = 0.8f;
    float laserShotTime = 0.15f;
    LineRenderer lineRenderer;
    GameObject barrel_end;
    bool drawLaser;
    bool fire;
    float laserThinSize = 0.0625f;
    float laserThickSize = 0.4375f;
    public AudioSource laserChargeSound;
    public AudioSource laserImpactSound;

    public Material laserAimMat;
    Color laserAimColor = new Color(1, 0, 0.2509804f);
    public Material laserShotMat;
    Color laserShotColor = new Color(1, 0.3349057f, 0.5f);


    public ContactFilter2D contact;

    void Start()
    {
        player = GameObject.Find("Player");
        barrel = transform.Find("barrel_thin").gameObject;
        barrel_end = barrel.transform.Find("barrel_end").gameObject;
        lineRenderer = transform.GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (CheckIfInSight())
        {
            //drawLaser = true;
            //lineRenderer.enabled = true;

            if (Time.time > shootTime + laserChargeTime + laserShotTime + coolDownTime)
            {
                shootTime = Time.time;
                StartCoroutine("Fire");
            }
            else if(!fire)
                Aim();
        }
        else
        {
            lineRenderer.enabled = false;
        }

        UpdateLaser();
    }

    void Aim()
    {
        Vector3 target = player.transform.position;
        target.z = 0f;

        Vector3 objectPosition = barrel.transform.position;
        target.x = target.x - objectPosition.x;
        target.y = target.y - objectPosition.y;

        float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        barrel.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    bool CheckIfInSight()
    {
        if (Physics2D.Raycast(barrel.transform.position, player.transform.position - barrel.transform.position, Mathf.Infinity, playerGroundMask).collider.tag == "Player")
        {
            return true;
        }
        return false;
    }

    IEnumerator Fire()
    {
        /*yield return new WaitForSeconds(laserChargeTime);
        fire = true;
        SetLaser(laserShotColor, laserThickSize, laserShotMat);
        yield return new WaitForSeconds(laserShotTime);
        fire = false;
        SetLaser(laserAimColor, laserThinSize, laserAimMat);*/
        laserChargeSound.Play();
        SetLaser(laserAimColor, laserThinSize, laserAimMat);
        lineRenderer.enabled = true;
        fire = true;
        yield return new WaitForSeconds(laserChargeTime);
        laserImpactSound.Play();
        SetLaser(laserShotColor, laserThickSize, laserShotMat);
        yield return new WaitForSeconds(laserShotTime);
        fire = false;
        lineRenderer.enabled = false;
    }

    void UpdateLaser()
    {
        Vector3 hit = Physics2D.Raycast(barrel.transform.position, barrel_end.transform.position - barrel.transform.position, Mathf.Infinity, groundMask).point;
        lineRenderer.SetPosition(0, barrel_end.transform.position);
        lineRenderer.SetPosition(1, hit);
    }

    void SetLaser(Color laserColor, float laserSize, Material laserMat)
    {
        lineRenderer.material = laserMat;
        lineRenderer.startWidth = laserSize;
        lineRenderer.endWidth = laserSize;
        lineRenderer.startColor = laserColor;
        lineRenderer.endColor = laserColor;
    }
}