using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerAnimation
{
    PickUp, Block, Taunt, Punch, Hit, StartMove, EndMove, Death, IsHit, KnockDown, ObjectCollision
}

public class PlayerControl : MonoBehaviour
{
    private const float SPEED = 5f;

    private Animator anim;
    private WeaponScript weapon;
    private UnarmedScript unarmed;
    private float attackDuration = 1f;
    private float blockDuration = 1f;
    private bool pickingUpWeapon;
    public GameObject weaponHand;
    public int playernumber;
    private StaminaSystem stamina;

    internal GameMaster gameMaster;

    private AudioSource audioSource;
    public AudioClip tauntSound;
    public AudioClip swordHitSound;
    public AudioClip kickSound;
    public AudioClip deathSound;
    public AudioClip receiveHitSound;
    public AudioClip knockdownSound;
    public AudioClip blockSound;
    public AudioClip pickupSound;
    public AudioClip collisionSound;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        Debug.Assert(anim != null, "No animator was found by PlayerControl, but is required!");

       // weapon = GetComponentInChildren<WeaponScript>();
       // Debug.Assert(weapon != null, "No WeaponScript was found in children of PlayerControl!");

        unarmed = GetComponentInChildren<UnarmedScript>();
        Debug.Assert(unarmed != null, "No UnarmedScript was found in children of PlayerControl!");

        stamina = GetComponentInChildren<StaminaSystem>();
        Debug.Assert(stamina != null, "No StaminaSystem was found in children of PlayerControl!");

        Debug.Assert(weaponHand != null, "No weaponHand was found");

        pickingUpWeapon = false;

        gameMaster = GetComponentInChildren<GameMaster>();
        Debug.Assert(gameMaster != null, "No game master was found by PlayerControl, but is required!");

        audioSource = GetComponentInChildren<AudioSource>();
        Debug.Assert(audioSource != null, "No AudioSource was found by PlayerControl, but is required!");

       
    }
    
    void Update()
    {
        // don't allow input or player control when game is not active or player is picking up weapon
        if (gameMaster != null && !gameMaster.IsGameActive())
        {
            TriggerAnimation(PlayerAnimation.EndMove);
            return;     
        }
        if (!IsPlayerAllowedToMove())
        {
            return;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Bump"))
        {
            Vector3 bumpMovement = new Vector3(-0.2f, 0f, 0f);
            transform.Translate(bumpMovement * Time.deltaTime * SPEED, Space.World);
            return;
        }

        float moveHorizontal = Input.GetAxisRaw("Horizontal_P"+playernumber);
        float moveVertical = Input.GetAxisRaw("Vertical_P" + playernumber);
        float hit = Input.GetAxis("Fire1_P" + playernumber);
        float taunt = Input.GetAxis("Fire3_P" + playernumber);
        float pickUp = Input.GetAxis("Jump_P" + playernumber);
        float block = Input.GetAxis("Fire2_P" + playernumber);

        if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("noInput"))
        {
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            if (movement.sqrMagnitude != 0)
            {
                TriggerAnimation(PlayerAnimation.StartMove);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movement), Time.deltaTime * 5);
                
                transform.Translate(movement.normalized * Time.deltaTime * SPEED, Space.World);
            }
            else
            {
                TriggerAnimation(PlayerAnimation.EndMove);
            }
       
            if (hit != 0)
            {
                if (weapon != null && !weapon.IsAttacking() && !weapon.IsBlocking()
                    && !anim.GetCurrentAnimatorStateInfo(0).IsName("attackWeapon"))
                {
                    if (stamina.HandleAttack(weapon))
                    {
                        weapon.InitiateAttack(attackDuration);
                        TriggerAnimation(PlayerAnimation.Hit);
                    }
                }
                else if(weapon == null && unarmed != null && !unarmed.IsAttacking())
                {
                    if (stamina.HandleAttack(null))
                    {
                        unarmed.InitiateAttack(attackDuration);
                        TriggerAnimation(PlayerAnimation.Punch);
                        
                    }
                }
            }

            if(taunt != 0)
            {
                TriggerAnimation(PlayerAnimation.Taunt);
            }

            if(pickUp != 0 && weapon == null && weaponHand != null
                && !anim.GetCurrentAnimatorStateInfo(0).IsName("Pickup"))
            {
              
                TriggerAnimation(PlayerAnimation.PickUp);
            }

            if(block != 0)
            {
                if(weapon != null 
                    && !weapon.IsAttacking()
                    && !weapon.IsBlocking())
                {
                    weapon.InitiateBlock(blockDuration);
                    TriggerAnimation(PlayerAnimation.Block);
                }
            }
        }
    }


    void OnCollisionStay(Collision collision)
    {
        WeaponScript weapon = collision.collider.GetComponent<WeaponScript>();
       
        if (collision.collider.tag == "weapon"
            && !weapon.isInUse)    
        {
            
            if (pickingUpWeapon)
            {
                PickUpWeapon(weapon);
            }
        }
        
    }

    void PickUpWeapon(WeaponScript pickUp)
    {
        pickUp.isInUse = true;
        pickUp.gameObject.transform.parent = weaponHand.transform;
        pickUp.gameObject.transform.localPosition = weaponHand.transform.localPosition  + pickUp.pickUpPos;
        pickUp.gameObject.transform.localEulerAngles = pickUp.pickUpRot;
        Destroy(pickUp.gameObject.GetComponent<Rigidbody>());
        this.weapon = pickUp;
        pickUp.RegisterPlayer(this);
        TriggerSoundEffect(PlayerAnimation.PickUp);
        this.pickingUpWeapon = false;
    }

    public void ActivatePickUp()
    {
        pickingUpWeapon = true;
    }

    public void DeactivatePickUp()
    {
        pickingUpWeapon = false;
    }

    public void TriggerAnimation(PlayerAnimation a)
    {
        if (anim != null)
        {
            switch (a)
            {
                case PlayerAnimation.PickUp:
                    anim.SetTrigger("PickUp");
                    break;
                case PlayerAnimation.Block:
                    anim.SetTrigger("Block");
                    break;
                case PlayerAnimation.Taunt:
                    anim.SetTrigger("Taunt");
                    break;
                case PlayerAnimation.Punch:
                    anim.SetTrigger("Punch");
                    break;
                case PlayerAnimation.Hit:
                    anim.SetTrigger("Hit");
                    break;
                case PlayerAnimation.StartMove:
                    anim.SetBool("Move", true);
                    break;
                case PlayerAnimation.EndMove:
                    anim.SetBool("Move", false);
                    break;
                case PlayerAnimation.Death:
                    anim.SetTrigger("Death");
                    break;
                case PlayerAnimation.IsHit:
                    anim.SetTrigger("isHit");
                    break;
                case PlayerAnimation.KnockDown:
                    anim.SetTrigger("knockDown");
                    break;
                case PlayerAnimation.ObjectCollision:
                    anim.SetTrigger("HitWall");
                    break;
                default:
                    Debug.LogWarning("No behavior specified for this animation type!");
                    break;
            }


            //TriggerSoundEffect(a);
        }
    }

    public void TriggerSoundEffect(PlayerAnimation a)
    {
        if (audioSource != null)
        {
            switch (a)
            {
                case PlayerAnimation.PickUp:    // called by script
                    audioSource.PlayOneShot(pickupSound);
                    break;
                case PlayerAnimation.Block:     // called by script
                    audioSource.PlayOneShot(blockSound);
                    break;
                case PlayerAnimation.Taunt:     // called by animation
                    audioSource.PlayOneShot(tauntSound);
                    break;
                case PlayerAnimation.Punch:     // called by animation
                    audioSource.PlayOneShot(kickSound);
                    break;
                case PlayerAnimation.Hit:       // called by script
                    audioSource.PlayOneShot(swordHitSound);
                    break;
                case PlayerAnimation.Death:     // called by script
                    audioSource.PlayOneShot(deathSound);
                    break;
                case PlayerAnimation.IsHit:     // called by script
                    audioSource.PlayOneShot(receiveHitSound);
                    break;
                case PlayerAnimation.KnockDown:     // called by animation
                    audioSource.PlayOneShot(knockdownSound);
                    break;
                case PlayerAnimation.ObjectCollision:       // TODO: call by animation
                    audioSource.PlayOneShot(collisionSound);
                    break;
                default:
                    break;
            }
        }
    }

    private bool IsPlayerAllowedToMove()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Pickup")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("taunt")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("isHit")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("Block")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("attackKick"))
        {
            return false;
        }
        return true;
    }


    public void AddAttackColliderToWeapon()     //TODO: rename this??
    {
        if(this.weapon != null)
        {
            //this.weapon.GetComponent<MeshCollider>().enabled = true;
            this.weapon.setAttacking(true);
        }
    }

    public void RemoveAttackColliderToWeapon()  //TODO: rename this??
    {
        if (this.weapon != null)
        {
           // this.weapon.GetComponent<MeshCollider>().enabled = false;
            this.weapon.setAttacking(false);
        }
    }

    public bool IsPlayerStumbling()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Bump");
    }
}
