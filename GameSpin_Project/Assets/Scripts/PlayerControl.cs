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
    private bool pickingUpWeapon;
    public GameObject weaponHand;
    public int playernumber;
    
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        Debug.Assert(anim != null, "No animator was found by PlayerControl, but is required!");

       // weapon = GetComponentInChildren<WeaponScript>();
       // Debug.Assert(weapon != null, "No WeaponScript was found in children of PlayerControl!");

        unarmed = GetComponentInChildren<UnarmedScript>();
        Debug.Assert(unarmed != null, "No UnarmedScript was found in children of PlayerControl!");

        Debug.Assert(weaponHand != null, "No weaponHand was found");

        pickingUpWeapon = false;

    }
    
    void Update()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal_P"+playernumber);
        float moveVertical = Input.GetAxisRaw("Vertical_P" + playernumber);
        float hit = Input.GetAxis("Fire1_P" + playernumber);
        float taunt = Input.GetAxis("Fire2_P" + playernumber);
        float pickUp = Input.GetAxis("Jump_P" + playernumber);

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

            if(taunt != 0)
            {

                if(anim != null)
                {
                    anim.SetTrigger("Taunt");
                }
            }

            if(pickUp != 0 && weapon == null && weaponHand != null)
            {
                this.ProcessPickUp(true);
            }else
            {
                this.ProcessPickUp(false);
            }

        }
    }


    void OnCollisionStay(Collision collision)
    {
        WeaponScript weapon = collision.collider.GetComponent<WeaponScript>();
       
        if (collision.collider.tag == "weapon")    
        {
            
            if (pickingUpWeapon)
            {
                pickUpWeapon(weapon);
            }
        }
        
    }

    void pickUpWeapon(WeaponScript pickUp)
    {
        pickUp.gameObject.transform.parent = weaponHand.transform;
        pickUp.gameObject.transform.localPosition = weaponHand.transform.localPosition  + pickUp.pickUpPos;
        pickUp.gameObject.transform.localEulerAngles = pickUp.pickUpRot;
        Destroy(pickUp.gameObject.GetComponent<Rigidbody>());
        this.weapon = pickUp;
        this.pickingUpWeapon = false;

    }

    private void ProcessPickUp( bool val)
    {
        pickingUpWeapon = val;
    }

}
