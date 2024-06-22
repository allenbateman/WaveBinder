using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactiveSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject Prefab;

    [SerializeField]
    float spawnThreshold;

    bool CanSpawn = true;
    public float SpawnValue
    {
        get { return spawnValue; }
        set { spawnValue = value; }
    }
    private float spawnValue;

    void Update()
    {
        if (CanSpawn && spawnThreshold <= spawnValue)
        {
            Instantiate(Prefab, transform.position, transform.rotation);
            CanSpawn = false;
        }
        else if (!CanSpawn && spawnValue <= 0.3)
            CanSpawn = true;
    }
}
