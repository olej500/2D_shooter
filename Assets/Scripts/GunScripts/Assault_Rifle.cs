using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assault_Rifle : Gun
{
    public override string GunName { get; set; } = "Assault rifle";
    public override int MagSize { get; set; } = 30;
    public override bool Automatic { get; set; } = true;
    public override int Damage { get; set; } = 25;
    public override float FireRate { get; set; } = 0.16f;
    public override float Accuracy { get; set; } = 2.5f;
    public override float ReloadTime { get; set; } = 1.5f;

    public void Start()
    {
        barrel = transform.Find("barrel");
        barrelEnd = barrel.transform.Find("barrel_end");
        gameObject.GetComponent<AudioSource>().clip = Resources.Load("AR_sound") as AudioClip;
    }

    public override void Fire(int damage, float accuracy, float localScale)
    {
        GameObject spawnedBullet = Instantiate(bullet, barrelEnd.transform.position, Quaternion.Euler(0, 0, barrelEnd.transform.eulerAngles.z + Random.Range(-accuracy, accuracy)));
        spawnedBullet.transform.localScale = new Vector3(spawnedBullet.transform.localScale.x * localScale, spawnedBullet.transform.localScale.y, spawnedBullet.transform.localScale.z);
        spawnedBullet.GetComponent<TrailRenderer>().startColor = Color.white;
        spawnedBullet.GetComponent<TrailRenderer>().endColor = new Color32(0, 44, 255, 255);
        spawnedBullet.GetComponent<Bullet>().damage = damage;
    }
}