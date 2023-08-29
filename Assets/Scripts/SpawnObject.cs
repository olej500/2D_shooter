using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    //public GameObject[] objects;
    [Serializable]
    public class GameObjectIntTuple
    {
        public GameObject item;
        public int chance;
    }
    public List<GameObjectIntTuple> items;

    void Start()
    {
        int[] chanceSum = new int[items.Count];
        int sum = 0;

        for (int i = 0; i < chanceSum.Length; i++)
        {
            chanceSum[i] = items[i].chance + sum;
            sum = chanceSum[i];
        }

        int rand = UnityEngine.Random.Range(0, 100);

        for (int i = 0; i < chanceSum.Length; i++)
        {
            if (rand < chanceSum[i])
            {
                Instantiate(items[i].item, new Vector3(transform.position.x, transform.position.y, transform.position.z - transform.position.x * 0.001f), Quaternion.identity);
                break;
            }
        }
    }
}