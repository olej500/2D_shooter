using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electric_Drone : MonoBehaviour
{
    Pathfinding2D pathfinding;
    public Transform target;
    float speed = 3;
    Vector3[] path;
    int targetIndex;


    // Start is called before the first frame update
    void Start()
    {
        pathfinding = GetComponent<Pathfinding2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
