using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour
{
    Rigidbody rb;
    ParticleSystem particles;
    bool AreCandlesLit = true;
    bool startUpWaiting = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        particles = GetComponentInChildren<ParticleSystem>();

        StartCoroutine(WaitForStartUp());
    }
    
    void Update()
    {
        if (AreCandlesLit
            && !startUpWaiting
            && particles != null
            && rb != null
            && !rb.IsSleeping())
        {
            particles.Stop();
            AreCandlesLit = false;
        }
    }

    private IEnumerator WaitForStartUp()
    {
        yield return new WaitForSeconds(2);

        startUpWaiting = false;
    }
}
