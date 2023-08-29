using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckVelocity : MonoBehaviour
{
    Rigidbody2D rb;
    public bool checkVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(checkVelocity)
        {
            Debug.Log("crateVelocityX: " + rb.velocity.x);
        }
    }
}
