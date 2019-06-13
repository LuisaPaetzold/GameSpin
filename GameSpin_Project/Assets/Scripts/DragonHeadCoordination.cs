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

    private float duration = 3;

    private MovingCamera cam;

    void Start()
    {
        dragons = GetComponentsInChildren<DragonHead>();

        currentRand = Random.Range(randomActionTimeMin, randomActionTimeMax);

        cam = FindObjectOfType<MovingCamera>();
        Debug.Assert(cam != null, "Dragon Head Coordination script did not find Camera in scene!");
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
        bool isOneHeadActive = false;
        foreach(DragonHead d in dragons)
        {
            float r = Random.value;
            if (r < fireChance)
            {
                isOneHeadActive = true;
                d.BreatheFire(duration);
            }
        }

        if (isOneHeadActive
            && cam != null)
        {
            cam.CameraRumbleForSeconds(duration);
        }

        timerLastAction = Time.timeSinceLevelLoad;
        currentRand = Random.Range(randomActionTimeMin, randomActionTimeMax);
    }
}
