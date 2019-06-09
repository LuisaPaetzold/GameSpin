using DigitalRuby.PyroParticles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonHead : MonoBehaviour
{
    public GameObject fireObject;
    private float duration = 3;

    public void BreatheFire()
    {
        Object.Instantiate(fireObject, this.transform);
        FireBaseScript fire = fireObject.GetComponent<FireBaseScript>();
        if (fire != null)
        {
            fire.Duration = duration;
        }
    }
}
