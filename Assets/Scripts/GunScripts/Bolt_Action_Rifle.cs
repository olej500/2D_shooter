using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt_Action_Rifle : Gun
{
    public override string GunName { get; set; } = "Sniper rifle";
    public override int MagSize { get; set; } = 1;
    public override bool Automatic { get; set; }
    public override int Damage { get; set; } = 100;
    public override float FireRate { get; set; } = 0.75f;
    public override float Accuracy { get; set; } = 0.5f;
    public override float ReloadTime { get; set; } = 1.5f;

    public void Start()
    {
        barrel = transform.Find("barrel");
        barrelEnd = barrel.transform.Find("barrel_end");
        gameObject.GetComponent<AudioSource>().clip = Resources.Load("Bolt_sound") as AudioClip;
    }

    public override void Fire(int damage, float accuracy, float localScale)
    {
        GameObject spawnedBullet = Instantiate(bullet, barrelEnd.transform.position, Quaternion.Euler(0, 0, barrelEnd.transform.eulerAngles.z + Random.Range(-accuracy, accuracy)));
        spawnedBullet.transform.localScale = new Vector3(spawnedBullet.transform.localScale.x * localScale, spawnedBullet.transform.localScale.y, spawnedBullet.transform.localScale.z);
        spawnedBullet.GetComponent<TrailRenderer>().startColor = Color.white;
        spawnedBullet.GetComponent<TrailRenderer>().endColor = new Color32(255, 44, 0, 255);
        spawnedBullet.GetComponent<TrailRenderer>().time = 0.16f;
        spawnedBullet.GetComponent<Bullet>().speed = 80f;
        spawnedBullet.GetComponent<Bullet>().damage = damage;
    }
}