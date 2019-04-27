using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{

    public int lives;

    private Animator anim;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        Debug.Assert(anim != null, "No animator was found by HealthSystem, but is required!");
    }

    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        WeaponScript weapon = collision.collider.GetComponent<WeaponScript>();
        UnarmedScript unarmed = collision.collider.GetComponent<UnarmedScript>();

        if (collision.collider.tag == "weapon"
            && weapon != null
            && weapon.IsAttacking())    // only take damage if hit by a weapon that's actually attacking
        {
            HandleAttack(weapon.GetDamage());
        }else if(collision.collider.tag == "unarmed" 
            && weapon == null 
            && unarmed.IsAttacking())
        {

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
        if (anim != null)
        {
            anim.SetTrigger("isHit");
        }
        
        if (lives <= 0)
        {
            HandleDeath();
        }
    }

    void HandleKnockdown()
    {
        if(anim != null)
        {
            anim.SetTrigger("knockDown");
        }
    }

}
