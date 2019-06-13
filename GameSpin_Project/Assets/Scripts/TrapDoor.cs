using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDoor : MonoBehaviour
{
    public AudioClip openSqueak;
    public AudioClip closeSqueak;

    private Animator anim;
    private AudioSource sfx;
    private bool isOpen;

    private float randomActionTimeMin = 15;
    private float randomActionTimeMax = 30;
    private float currentRand;
    private float timerLastAction;

    void Start()
    {
        anim = GetComponent<Animator>();
        Debug.Assert(anim != null, "No Animator was found by Trap Door but is required!" + gameObject.name);

        sfx = GetComponent<AudioSource>();
        Debug.Assert(sfx != null, "Trap door script did not find Audio Source!");

        currentRand = Random.Range(randomActionTimeMin, randomActionTimeMax);
    }

    void Update()
    {
        if(Time.timeSinceLevelLoad - timerLastAction > currentRand)
        {
            HandleAction();
        }
    }

    public void HandleAction()
    {
        if (isOpen)
        {
            CloseTrapDoor();
        }
        else
        {
            OpenTrapDoor();
        }
        timerLastAction = Time.timeSinceLevelLoad;
        currentRand = Random.Range(randomActionTimeMin, randomActionTimeMax);
    }

    void OpenTrapDoor()
    {
        if (anim != null
            && !isOpen)
        {
            isOpen = true;
            anim.SetTrigger("Open");
            if (sfx != null)
            {
                sfx.PlayOneShot(openSqueak);
            }
        }
    }

    void CloseTrapDoor()
    {
        if (anim != null
            && isOpen)
        {
            isOpen = false;
            anim.SetTrigger("Close");

            if (sfx != null)
            {
                sfx.PlayOneShot(closeSqueak);
            }
        }
    }
}
