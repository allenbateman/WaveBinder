using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactiveColor : MonoBehaviour
{
    MeshRenderer meshRenderer;
    public Color Color
    {
        get { return color; }
        set { color = value; }
    }
    private Color color;
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();    
    }
    void Update()
    {
        meshRenderer.material.color = color;
    }
}
