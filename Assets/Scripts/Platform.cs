using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public int size;
    public BoxCollider2D mainCollider;
    public GameObject platformGameObject;
    Collider2D playerCollider;
    GameObject player;
    GameObject groundCheckStart;
    GameObject groundCheckEnd;
    GameObject levelCheck;

    [SerializeField]
    public LayerMask groundMask;

    void Start()
    {
        player = GameObject.Find("Player");
        playerCollider = player.GetComponent<BoxCollider2D>();
        groundCheckStart = player.transform.Find("GroundCheckStart").gameObject;
        groundCheckEnd = player.transform.Find("GroundCheckEnd").gameObject;
        levelCheck = transform.Find("levelCheck").gameObject;

        if(size % 2 == 0)
        {
            for (int i = 0; i < size; i++)
            {
                Instantiate(platformGameObject, new Vector3(transform.position.x - size / 2 + 0.5f + i, transform.position.y, transform.position.z), Quaternion.identity);
            }
            mainCollider.size = new Vector2(size, mainCollider.size.y);
        }
        else
        {
            for (int i = 0; i < size; i++)
            {
                Instantiate(platformGameObject, new Vector3(transform.position.x - size / 2 + i, transform.position.y, transform.position.z), Quaternion.identity);
            }
            mainCollider.size = new Vector2(size, mainCollider.size.y);
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.S) && player.GetComponent<PlayerScript>().lastOnGroundTime > 0 && (Physics2D.OverlapCircle(groundCheckStart.transform.position, 0.05f, groundMask) == mainCollider || Physics2D.OverlapCircle(groundCheckEnd.transform.position, 0.05f, groundMask) == mainCollider))
        {
            Physics2D.IgnoreCollision(playerCollider, mainCollider, true);
        }
        else if(groundCheckEnd.transform.position.y < levelCheck.transform.position.y)
        {
            Physics2D.IgnoreCollision(playerCollider, mainCollider, true);
        }
        else
            Physics2D.IgnoreCollision(playerCollider, mainCollider, false);
    }
}