using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCollider : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        HealthSystem player = collision.transform.GetComponent<HealthSystem>();
        if (player == null)
        {
            // destroy every object but not the player
            Destroy(collision.gameObject);
        }
    }
}
