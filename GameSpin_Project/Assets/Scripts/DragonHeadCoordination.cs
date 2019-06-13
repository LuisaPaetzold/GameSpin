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
    private AudioSource sfx;

    void Start()
    {
        dragons = GetComponentsInChildren<DragonHead>();

        currentRand = Random.Range(randomActionTimeMin, randomActionTimeMax);

        cam = FindObjectOfType<MovingCamera>();
        Debug.Assert(cam != null, "Dragon Head Coordination script did not find Camera in scene!");

        sfx = GetComponent<AudioSource>();
        Debug.Assert(sfx != null, "Dragon Head Coordination script did not find Audio Source!");
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
            StartCoroutine(TriggerFireSfx());
        }

        timerLastAction = Time.timeSinceLevelLoad;
        currentRand = Random.Range(randomActionTimeMin, randomActionTimeMax);
    }

    private IEnumerator TriggerFireSfx()
    {
        if (sfx != null)
        {
            sfx.Play();

            yield return new WaitForSeconds(duration);

            sfx.Stop();
        }
    }
}
