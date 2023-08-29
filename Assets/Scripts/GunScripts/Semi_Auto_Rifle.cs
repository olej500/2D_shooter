using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Semi_Auto_Rifle : Gun
{
    public override string GunName { get; set; } = "Semi-auto rifle";
    public override int MagSize { get; set; } = 12;
    public override bool Automatic { get; set; }
    public override int Damage { get; set; } = 60;
    public override float FireRate { get; set; } = 0.22f;
    public override float Accuracy { get; set; } = 1.5f;
    public override float ReloadTime { get; set; } = 1.5f;

    public void Start()
    {
        barrel = transform.Find("barrel");
        barrelEnd = barrel.transform.Find("barrel_end");
        gameObject.GetComponent<AudioSource>().clip = Resources.Load("Semi_sound") as AudioClip;
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