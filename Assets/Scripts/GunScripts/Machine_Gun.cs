using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine_Gun : Gun
{
    public override string GunName { get; set; } = "Light machine gun";
    public override int MagSize { get; set; } = 50;
    public override bool Automatic { get; set; } = true;
    public override int Damage { get; set; } = 15;
    public override float FireRate { get; set; } = 0.08f;
    public override float Accuracy { get; set; } = 4.5f;
    public override float ReloadTime { get; set; } = 1.5f;

    public void Start()
    {
        barrel = transform.Find("barrel");
        barrelEnd = barrel.transform.Find("barrel_end");
        gameObject.GetComponent<AudioSource>().clip = Resources.Load("MG_sound") as AudioClip;
        FireRate = 0.01f;
    }

    public override void Fire(int damage, float accuracy, float localScale)
    {
        GameObject spawnedBullet = Instantiate(bullet, barrelEnd.transform.position, Quaternion.Euler(0, 0, barrelEnd.transform.eulerAngles.z + Random.Range(-accuracy, accuracy)));
        spawnedBullet.transform.localScale = new Vector3(spawnedBullet.transform.localScale.x * localScale, spawnedBullet.transform.localScale.y, spawnedBullet.transform.localScale.z);
        spawnedBullet.GetComponent<TrailRenderer>().startColor = Color.white;
        spawnedBullet.GetComponent<TrailRenderer>().endColor = new Color32(255, 44, 0, 255);
        spawnedBullet.GetComponent<Bullet>().damage = damage;
    }
}