using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonHeadCoordination : MonoBehaviour
{
    DragonHead[] dragons;

    private float randomActionTimeMin = 15;
    private float randomActionTimeMax = 30;
    private float currentRand;
    private float timerLastAction;
    private float fireChance = 0.3f;
    
    void Start()
    {
        dragons = GetComponentsInChildren<DragonHead>();

        currentRand = Random.Range(randomActionTimeMin, randomActionTimeMax);
    }
    
    void Update()
    {
        if (Time.timeSinceLevelLoad - timerLastAction > currentRand)
        {
            HandleAction();
        }
    }

    void HandleAction()
    {
        foreach(DragonHead d in dragons)
        {
            float r = Random.value;
            if (r < fireChance)
            {
                d.BreatheFire();
            }
        }

        timerLastAction = Time.timeSinceLevelLoad;
        currentRand = Random.Range(randomActionTimeMin, randomActionTimeMax);
    }
}
