using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket_Script : MonoBehaviour
{
    public float rotationSpeed;
    public float speed;
    GameObject player;
    Rigidbody2D rb;
    public GameObject explosion;
    public float destroyTime;
    public ParticleSystem emit;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        rb = gameObject.GetComponent<Rigidbody2D>();
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        //rotate
        Vector2 direction = (Vector2)player.transform.position - rb.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.right).z;

        if (Vector3.Angle(direction, transform.right) > 90)
        {
            if(Vector3.Cross(direction, transform.right).z > 0)
                rotateAmount = 1;
            else
                rotateAmount = -1;
        }
        rb.angularVelocity = -rotationSpeed * rotateAmount;
    }

    private void FixedUpdate()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        DetachParticles();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Instantiate(explosion, transform.position, transform.rotation);
    }

    public void DetachParticles()
    {
        emit.transform.parent = null;
        ParticleSystem.EmissionModule emission = emit.emission;
        emission.enabled = false;
        Destroy(emit.gameObject, 1f);
    }
}