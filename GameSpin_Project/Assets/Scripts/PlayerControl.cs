using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : MonoBehaviour
{
    private const float SPEED = 5f;

    private Animator anim;
    private WeaponScript weapon;
    private UnarmedScript unarmed;
    private float attackDuration = .5f;
    
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        Debug.Assert(anim != null, "No animator was found by PlayerControl, but is required!");

       // weapon = GetComponentInChildren<WeaponScript>();
       // Debug.Assert(weapon != null, "No WeaponScript was found in children of PlayerControl!");

        unarmed = GetComponentInChildren<UnarmedScript>();
        Debug.Assert(unarmed != null, "No UnarmedScript was found in children of PlayerControl!");
    }
    
    void Update()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        float hit = Input.GetAxis("Fire1");

        if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("noInput"))
        {
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            if (movement.sqrMagnitude != 0)
            {
                if (anim != null)
                {
                    anim.SetBool("Move", true);
                }
                transform.rotation = Quaternion.LookRotation(movement);
                transform.Translate(movement * Time.deltaTime * SPEED, Space.World);
            }
            else
            {
                if (anim != null)
                {
                    anim.SetBool("Move", false);
                }
            }
       
            if (hit != 0)
            {
                if (weapon != null && !weapon.IsAttacking())
                {
                    weapon.InitiateAttack(attackDuration);
                    if (anim != null)
                    {
                        anim.SetTrigger("Hit");
                    }
                }
                else if(weapon == null && unarmed != null && !unarmed.IsAttacking())
                {
                    unarmed.InitiateAttack(attackDuration);
                    if (anim != null)
                    {
                        anim.SetTrigger("Punch");
                    }
                }
            }
        }
    }
}
