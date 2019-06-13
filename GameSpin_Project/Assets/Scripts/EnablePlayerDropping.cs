using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnablePlayerDropping : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        PlayerControl player = other.transform.parent.GetComponent<PlayerControl>();
        if (player != null)
        {
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        PlayerControl player = other.transform.parent.GetComponent<PlayerControl>();
        if (player != null)
        {
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            }
        }
    }
}
