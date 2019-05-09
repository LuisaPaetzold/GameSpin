using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionBounce : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        PlayerControl player = collision.transform.GetComponent<PlayerControl>();
        if (player != null
            && !player.IsPlayerStumbling())
        {
            player.TriggerAnimation(PlayerAnimation.ObjectCollision);
        }
    }
}
