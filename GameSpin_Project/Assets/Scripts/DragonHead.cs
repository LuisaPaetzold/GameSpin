using DigitalRuby.PyroParticles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonHead : MonoBehaviour
{
    public GameObject fireObject;

    public void BreatheFire(float duration)
    {
        Object.Instantiate(fireObject, this.transform);
        FireBaseScript fire = fireObject.GetComponent<FireBaseScript>();
        if (fire != null)
        {
            fire.Duration = duration;
        }
    }
}
