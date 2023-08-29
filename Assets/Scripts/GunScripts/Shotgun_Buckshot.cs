using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun_Buckshot : Gun
{
    public override string GunName { get; set; } = "Shotgun";
    public override int MagSize { get; set; } = 8;
    public override bool Automatic { get; set; }
    public override int Damage { get; set; } = 25;
    public override float FireRate { get; set; } = 0.4f;
    public override float Accuracy { get; set; } = 6.0f;
    public override float ReloadTime { get; set; } = 1.5f;


    public void Start()
    {
        barrel = transform.Find("barrel");
        barrelEnd = barrel.transform.Find("barrel_end");
        gameObject.GetComponent<AudioSource>().clip = Resources.Load("Buckshot_sound") as AudioClip;
    }

    public override void Fire(int damage, float accuracy, float localScale)
    {
        for (int i = 0; i < 6; i++)
        {
            GameObject spawnedBullet = Instantiate(bullet, barrelEnd.transform.position, Quaternion.Euler(0, 0, barrelEnd.transform.eulerAngles.z + Random.Range(-accuracy, accuracy)));
            spawnedBullet.transform.localScale = new Vector3(spawnedBullet.transform.localScale.x * localScale, spawnedBullet.transform.localScale.y, spawnedBullet.transform.localScale.z);
            spawnedBullet.GetComponent<TrailRenderer>().startColor = Color.white;
            spawnedBullet.GetComponent<TrailRenderer>().endColor = new Color32(0, 44, 255, 255);
            spawnedBullet.GetComponent<TrailRenderer>().time = 0.04f;
            spawnedBullet.GetComponent<TrailRenderer>().startWidth = spawnedBullet.GetComponent<TrailRenderer>().startWidth / 2;
            spawnedBullet.transform.localScale = new Vector3(spawnedBullet.transform.localScale.x / 2, spawnedBullet.transform.localScale.y / 2, spawnedBullet.transform.localScale.z);
            spawnedBullet.GetComponent<Bullet>().damage = damage;
        }
    }
}
