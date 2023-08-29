using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    GameObject player;
    Rigidbody2D rb;

    public float speed = 50f;
    public int damage;

    void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed * player.transform.localScale.x;
        Destroy(gameObject, 1f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(gameObject);
    }

    void Update()
    {
        
    }
}
