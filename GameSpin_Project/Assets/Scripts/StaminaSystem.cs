using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaSystem : MonoBehaviour
{
    public int stamina = 100;
    public GameObject StaminaBar;

    private float timerLastAttack;

    private int maxStamina;

    void Start()
    {
        maxStamina = stamina;
    }

    void Update()
    {
        if(stamina < maxStamina)
        {
            if(Time.timeSinceLevelLoad - timerLastAttack > 3)
            {
                stamina++;
                UpdateStaminaBar();
            }
        }
    }

    void LateUpdate()
    {
        if (StaminaBar != null)
        {
            Vector3 fwd = Camera.main.transform.forward;
            fwd.y = 0; 
            StaminaBar.transform.parent.rotation = Quaternion.LookRotation(fwd);
        }
    }

    public bool HandleAttack(WeaponScript weapon)
    {
        if(weapon != null)
        { if(stamina >= weapon.staminaDrain)
            {
                this.stamina -= weapon.staminaDrain;
                this.timerLastAttack = Time.timeSinceLevelLoad;
                UpdateStaminaBar();
                return true;
            }
            return false;
        }

        if(stamina >= 5)
        {
            this.stamina -= 5;
            this.timerLastAttack = Time.timeSinceLevelLoad;
            UpdateStaminaBar();
            return true;
        }

        return false;
    }

    void UpdateStaminaBar()
    {
        if (StaminaBar != null)
        {
            float percentage = stamina * 1.0f / maxStamina * 1.0f;
            StaminaBar.transform.localScale = new Vector3(percentage, StaminaBar.transform.localScale.y, StaminaBar.transform.localScale.z);
        }
    }
}
