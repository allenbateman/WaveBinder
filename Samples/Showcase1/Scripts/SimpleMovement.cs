using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    [SerializeField]
    float speed;
    void Update()
    {
        transform.Translate(transform.up * speed * Time.deltaTime);
    }
}
