﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{

    public int lives;

    public GameObject HealthBar;

    private int maxLives;
    private Animator anim;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        Debug.Assert(anim != null, "No animator was found by HealthSystem, but is required!");

        maxLives = lives;
    }

    void Update()
    {
        
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
        if (collision.collider.tag == "weapon"
            && weapon != null
            && weapon.IsAttacking())    // only take damage if hit by a weapon that's actually attacking
        {
            HandleAttack(weapon.GetDamage());
        }
    }

    void HandleDeath()
    {
        Destroy(gameObject, 10);
        if (anim != null)
        {
            anim.SetBool("Death", true);
        }
    }

    void HandleAttack(int damage)
    {
        this.lives -= damage;
        if (lives < 0)
        {
            lives = 0;
        }
        UpdateHealthBar();

        if (anim != null)
        {
            anim.SetTrigger("isHit");
        }

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
