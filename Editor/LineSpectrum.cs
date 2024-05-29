using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using WaveBinder;

public class LineSpectrum : MonoBehaviour
{
    LineRenderer lineRenderer;
    AudioAnalyzer analyzer;
    public float sizeMultiplier = 100;
    // Property binders
    [SerializeReference] PropertyBinder[] _propertyBinders = null;
    public PropertyBinder[] propertyBinders
    {
        get => (PropertyBinder[])_propertyBinders.Clone();
        set => _propertyBinders = value;
    }
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();    
        analyzer = GetComponent<AudioAnalyzer>();

        lineRenderer.positionCount = analyzer.nSamples;
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0;i < analyzer.nSamples;i++)
        {
            lineRenderer.SetPosition(i, new Vector3(transform.position.x + i, analyzer.GetSpectrumData()[i]* sizeMultiplier, transform.position.z));
        }
    }
}
