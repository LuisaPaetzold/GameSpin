using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int lives;
    public GameObject HealthBar;

    private int maxLives;
    private PlayerControl player;
    private GameMaster gameMaster;

    void Start()
    {
        player = gameObject.GetComponent<PlayerControl>();
        Debug.Assert(player != null, "No PlayerControl was found by HealthSystem, but is required!");

        gameMaster = FindObjectOfType<GameMaster>();
        Debug.Assert(gameMaster != null, "No game master was found by HealthSystem, but is required!");

        maxLives = lives;
    }

    void LateUpdate()
    {
        if (HealthBar != null)
        {
            // update ui in LateUpdate to avoid jittering
            Vector3 fwd = Camera.main.transform.forward;
            fwd.y = 0;  // do not rotate on y to not tilt health bar
            HealthBar.transform.parent.rotation = Quaternion.LookRotation(fwd);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        WeaponScript weapon = collision.collider.GetComponent<WeaponScript>();
        UnarmedScript unarmed = collision.collider.GetComponent<UnarmedScript>();

        WeaponScript myweapon = this.gameObject.GetComponentInChildren<WeaponScript>();

        if (lives > 0)
        {
            if (collision.collider.tag == "weapon"
                && weapon != null
                && weapon.IsAttacking())    // only take damage if hit by a weapon that's actually attacking
            {
                if(myweapon!=null && myweapon.IsBlocking())
                {

                }else
                {
                    HandleAttack(weapon);
                }
                
            }
            else if (collision.collider.tag == "unarmed"
                && unarmed != null
                && unarmed.IsAttacking()
                && player != null)
            {
                player.TriggerAnimation(PlayerAnimation.KnockDown);
            }
        }
    }

    void HandleDeath()
    {
        if (gameMaster != null)
        {
            gameMaster.UnregisterPlayerAfterDeath(this);
        }

        Destroy(gameObject, 10);
        if (player != null)
        {
            player.TriggerAnimation(PlayerAnimation.Death);
        }
    }

    void HandleAttack(WeaponScript weapon)
    {
        weapon.HandleSuccessfulAttack();    // reset boolean in weapon to avoid accidental double hits

        if (player != null && this.lives > 0)
        {
            player.TriggerAnimation(PlayerAnimation.IsHit);
        }

        this.lives -= weapon.GetDamage();


        if (lives < 0)
        {
            lives = 0;
        }
        UpdateHealthBar();

        

        if (lives <= 0)
        {
            HandleDeath();
        }
    }

    void UpdateHealthBar()
    {
        if (HealthBar != null)
        {
            float percentage = lives * 1.0f / maxLives * 1.0f;
            HealthBar.transform.localScale = new Vector3(percentage, HealthBar.transform.localScale.y, HealthBar.transform.localScale.z);
        }
    }
}
