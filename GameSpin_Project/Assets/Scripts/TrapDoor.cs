using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDoor : MonoBehaviour
{
    private Animator anim;
    private bool isOpen;

    private float randomActionTimeMin = 15;
    private float randomActionTimeMax = 30;
    private float currentRand;
    private float timerLastAction;

    void Start()
    {
        anim = GetComponent<Animator>();
        Debug.Assert(anim != null, "No Animator was found by Trap Door but is required!" + gameObject.name);

        currentRand = Random.Range(randomActionTimeMin, randomActionTimeMax);
        Debug.Log(currentRand);
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
        Debug.Log(currentRand);
    }

    void OpenTrapDoor()
    {
        if (anim != null
            && !isOpen)
        {
            isOpen = true;
            anim.SetTrigger("Open");
        }
    }

    void CloseTrapDoor()
    {
        if (anim != null
            && isOpen)
        {
            isOpen = false;
            anim.SetTrigger("Close");
        }
    }
}
