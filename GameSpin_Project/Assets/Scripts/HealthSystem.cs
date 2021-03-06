﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float lives;
    public GameObject HealthBar;

    private float maxLives;
    private PlayerControl player;
    private GameMaster gameMaster;
    private Camera cam;

    void Start()
    {
        player = gameObject.GetComponent<PlayerControl>();
        Debug.Assert(player != null, "No PlayerControl was found by HealthSystem, but is required!");

        gameMaster = FindObjectOfType<GameMaster>();
        Debug.Assert(gameMaster != null, "No game master was found by HealthSystem, but is required!");

        cam = FindObjectOfType<Camera>();
        Debug.Assert(cam != null, "No game camera was found by HealthSystem, but is required!");

        maxLives = lives;
    }

    void LateUpdate()
    {
        if (HealthBar != null)
        {
            // update ui in LateUpdate to avoid jittering
            Vector3 fwd = cam.transform.forward;
            fwd.y = 0;  // do not rotate on y to not tilt health bar
            HealthBar.transform.parent.rotation = Quaternion.LookRotation(fwd);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        WeaponScript weapon = collision.collider.GetComponent<WeaponScript>();
        UnarmedScript unarmed = collision.collider.GetComponent<UnarmedScript>();
        DeathCollider death = collision.collider.GetComponent<DeathCollider>();

        WeaponScript myweapon = this.gameObject.GetComponentInChildren<WeaponScript>();

        if (lives > 0)
        {
            if (collision.collider.tag == "weapon"
                && weapon != null
                && weapon.IsAttacking())    // only take damage if hit by a weapon that's actually attacking
            {
                if(myweapon!=null && myweapon.IsBlocking())
                {
                    // knock attacking player down if attack was blocked successfully

                    PlayerControl p = weapon.GetRegisteredPlayer();
                    if (p != null)
                    {
                        //p.TriggerAnimation(PlayerAnimation.KnockDown);
                        p.DropWeapon();
                    }

                    player.TriggerSoundEffect(PlayerAnimation.Block);
                }
                else
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
            else if(death != null)
            {
                // touching death collider means instant death
                lives = 0;
                HandleDeath();
            }
        }
    }

    public void HandleDeath()
    {
        if (gameMaster != null)
        {
            gameMaster.UnregisterPlayerAfterDeath(this);
        }

        Destroy(gameObject, 10);
        if (player != null)
        {
            player.TriggerAnimation(PlayerAnimation.Death);
            player.TriggerSoundEffect(PlayerAnimation.Death);
        }
    }

    void HandleAttack(WeaponScript weapon)
    {
        weapon.HandleSuccessfulAttack(player);    // reset boolean in weapon to avoid accidental double hits

        if (player != null && this.lives > 0)
        {
            player.TriggerAnimation(PlayerAnimation.IsHit);
            player.TriggerSoundEffect(PlayerAnimation.IsHit);
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

    void OnParticleCollision(GameObject TargetedParticle)
    {
        this.lives -= 0.1f;


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

}
