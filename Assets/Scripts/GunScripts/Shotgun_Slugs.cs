using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun_Slugs : Gun
{
    public override string GunName { get; set; } = "Grenade launcher";
    public override int MagSize { get; set; } = 5;
    public override bool Automatic { get; set; }
    public override int Damage { get; set; } = 80;
    public override float FireRate { get; set; } = 0.6f;
    public override float Accuracy { get; set; } = 3.0f;
    public override float ReloadTime { get; set; } = 1.5f;

    public void Start()
    {
        barrel = transform.Find("barrel");
        barrelEnd = barrel.transform.Find("barrel_end");
        gameObject.GetComponent<AudioSource>().clip = Resources.Load("Slug_sound") as AudioClip;
    }

    public override void Fire(int damage, float accuracy, float localScale)
    {
        GameObject spawnedBullet = Instantiate(bullet, barrelEnd.transform.position, Quaternion.Euler(0, 0, barrelEnd.transform.eulerAngles.z + Random.Range(-accuracy, accuracy)));
        spawnedBullet.transform.localScale = new Vector3(spawnedBullet.transform.localScale.x * localScale, spawnedBullet.transform.localScale.y, spawnedBullet.transform.localScale.z);
        spawnedBullet.GetComponent<TrailRenderer>().startColor = Color.white;
        spawnedBullet.GetComponent<TrailRenderer>().endColor = new Color32(255, 44, 0, 255);
        spawnedBullet.GetComponent<TrailRenderer>().time = 0.056f;
        spawnedBullet.GetComponent<TrailRenderer>().startWidth = spawnedBullet.GetComponent<TrailRenderer>().startWidth * 1.75f;
        spawnedBullet.transform.localScale = new Vector3(spawnedBullet.transform.localScale.x * 1.75f, spawnedBullet.transform.localScale.y * 1.75f, spawnedBullet.transform.localScale.z);
        spawnedBullet.GetComponent<Rigidbody2D>().gravityScale = 1.75f;
        spawnedBullet.GetComponent<Bullet>().speed = 35f;
        spawnedBullet.GetComponent<Bullet>().damage = damage;
    }
}