using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactiveSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject Prefab;

    [SerializeField]
    float spawnThreshold;

    public float SpawnValue
    {
        get { return spawnValue; }
        set { spawnValue = value; }
    }
    private float spawnValue;

    void Update()
    {
        if(spawnThreshold < spawnValue)
        {
            Instantiate(Prefab,transform.position,transform.rotation);
        }
    }
}
