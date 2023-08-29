using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    [HideInInspector]
    public Transform barrel;
    [HideInInspector]
    public Transform barrelEnd;
    [HideInInspector]
    public GameObject bullet;

    public int fireRateCounter;
    public int accuracyCounter;
    public int reloadTimeCounter;

    public virtual string GunName { get; set; }
    public virtual int MagSize { get; set; }
    public virtual bool Automatic { get; set; }
    public virtual int Damage { get; set; }
    public virtual float FireRate { get; set; }
    public virtual float Accuracy { get; set; }
    public virtual float ReloadTime { get; set; }

    public void Awake()
    {
        bullet = Resources.Load("bullet") as GameObject;
        gameObject.AddComponent<AudioSource>();
    }

    public abstract void Fire(int damage, float accuracy, float localScale);
}