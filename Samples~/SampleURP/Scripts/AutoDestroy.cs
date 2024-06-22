using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField]
    private float destroyTime = 1f;
    private float timer = 0f;
    void Update()
    {
        timer += Time.deltaTime;

        if(timer > destroyTime)
        {
            Destroy(gameObject);
        }
    }
}
