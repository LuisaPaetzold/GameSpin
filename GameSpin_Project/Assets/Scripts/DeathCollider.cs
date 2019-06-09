using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCollider : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        HealthSystem player = collision.transform.GetComponent<HealthSystem>();
        if (player != null)
        {
            player.HandleDeath();
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
}
