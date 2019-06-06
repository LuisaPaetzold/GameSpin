using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObjects : MonoBehaviour
{

    private AudioSource audioSource;
    private int collisionCount;
    public AudioClip collisionSound;
    public AudioClip ScratchSound;
    

    void Start()
    {
        audioSource = GetComponentInChildren<AudioSource>();
        Debug.Assert(audioSource != null, "No AudioSource was found by LevelObject but is required!" + gameObject.name);
    }

    bool noContact()
    {
        return collisionCount == 0;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (audioSource != null)
        {
            if (noContact())
            {
                audioSource.PlayOneShot(collisionSound, 0.1f);
            }
            else
            {
                audioSource.PlayOneShot(ScratchSound, 0.1f);
            }
        }
        
        collisionCount++;
    }

    void OnCollisionExit(Collision collision)
    {
        collisionCount--;
    }
}
