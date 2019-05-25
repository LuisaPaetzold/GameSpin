using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObjects : MonoBehaviour
{

    private AudioSource audioSource;
    private int collisionCount;
    public AudioClip collisionSound;
    public AudioClip ScratchSound;
    


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponentInChildren<AudioSource>();
        Debug.Assert(audioSource != null, "No AudioSource was found by LevelObject but is required!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool noContact()
    {
        return collisionCount == 0;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (noContact())
            audioSource.PlayOneShot(collisionSound);
        else
            audioSource.PlayOneShot(ScratchSound);
        collisionCount++;
    }

    void OnCollisionExit(Collision collision)
    {
        collisionCount--;
    }
}
