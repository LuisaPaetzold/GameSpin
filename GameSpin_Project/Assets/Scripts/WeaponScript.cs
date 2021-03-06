﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public int damage;
    public int staminaDrain;
    public float attackSpeedMultiplier = 1;
    private bool attackInProgress;
    private bool blockInProgress;
    public bool isInUse;
    public Vector3 pickUpPos;
    public Vector3 pickUpRot;

    private PlayerControl player;
    private ParticleSystem glow;
    private AudioSource sound;

    void Start()
    {
        glow = GetComponentInChildren<ParticleSystem>();

        sound = GetComponentInChildren<AudioSource>();

    }

    public int GetDamage()
    {
        return damage;
    }

    public void InitiateAttack(float duration)
    {
        StartCoroutine("ProcessAttack", duration);
    }

    public void InitiateBlock(float duration)
    {
        StartCoroutine("ProcessBlock", duration);
    }

    private IEnumerator ProcessAttack(float duration)
    {
        // let weapon know that an attack is in progress

        attackInProgress = true;

        yield return new WaitForSeconds(duration);

        attackInProgress = false;
    }

    private IEnumerator ProcessBlock(float duration)
    {
        blockInProgress = true;

        yield return new WaitForSeconds(duration);

        blockInProgress = false;
    }


    public bool IsAttacking()
    {
        return attackInProgress;
    }

    public bool IsBlocking()
    {
        return blockInProgress;
    }

    public void HandleSuccessfulAttack(PlayerControl player)
    {
        attackInProgress = false;
        player.TriggerSoundEffect(PlayerAnimation.Hit);
    }

    void OnCollisionEnter(Collision collision)
    {
        WeaponScript weapon = collision.collider.GetComponent<WeaponScript>();

        if (collision.collider.tag == "weapon"
            && weapon.isInUse && weapon.IsBlocking() && this.IsAttacking())
        {
            transform.parent = null;
            gameObject.AddComponent<Rigidbody>();
        }
    }


    public void RegisterPlayer(PlayerControl p)
    {
        if (p != null)
        {
            player = p;
            p.SetWeaponAttackSpeedMultiplier(attackSpeedMultiplier);
        }
        if (glow != null)
        {
            glow.Stop();
        }
        if(sound != null)
        {
            sound.enabled = false;
        }

    }

    public void DeregisterPlayer()
    {
        player.SetWeaponAttackSpeedMultiplier(1);
        player = null;
        if (glow != null)
        {
            glow.Play();
        }
        if (sound != null)
        {
            sound.enabled = true;
        }
    }

    public PlayerControl GetRegisteredPlayer()
    {
        return player;
    }

    public void setAttacking(bool val)
    {
        this.attackInProgress = val;
    }
}
