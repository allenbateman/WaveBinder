using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ReactiveParticleSystem : MonoBehaviour
{
    ParticleSystem particleSystem;
    private ParticleSystem.Burst burst;
    private bool hasBurstTriggered = false;
    public float SpawnValue
    {
        get { return spawnValue; }
        set { spawnValue = value; }
    }
    private float spawnValue;

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
        var emission = particleSystem.emission;
        burst = new ParticleSystem.Burst(0, 1);
        emission.SetBursts(new ParticleSystem.Burst[] { burst });
    }

    private void Update()
    {
        if (spawnValue >= 0.8f && !hasBurstTriggered)
        {
            TriggerBurst();
            hasBurstTriggered = true;
        }else if( spawnValue <= 0.3f && hasBurstTriggered)
        {
            hasBurstTriggered = false;
        }
    }
    void TriggerBurst()
    {
        particleSystem.Emit(burst.minCount);
    }
}
