using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaSystem : MonoBehaviour
{
    public int stamina = 100;
    public GameObject StaminaBar;

    private float timerLastAttack;
    private bool rechargingBlock;

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
                if (rechargingBlock && stamina == maxStamina)
                {
                    SetRechargingBlock(false);
                }
                UpdateStaminaBar();
            }
        }
    }

    public bool HandleAttack(WeaponScript weapon)
    {
        if(rechargingBlock)
        {
            // there's still an active block, we cannot attack!
            return false;
        }

        int drain = 5;   // stamina drain while kicking
        if (weapon != null)
        {
            drain = weapon.staminaDrain;
        }

        if (stamina >= drain)
        {
            this.stamina -= drain;
            this.timerLastAttack = Time.timeSinceLevelLoad;
            UpdateStaminaBar();
            return true;
        }
        else
        {
            // we did not have enough stamina but want to use the rest we have
            // this leaves us with 0 stamina and a block: we cannot do anything until stamina fully recharged
            if (stamina > 0 && !rechargingBlock)
            {
                stamina = 0;
                SetRechargingBlock(true);
                UpdateStaminaBar();
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    void UpdateStaminaBar()
    {
        if (StaminaBar != null)
        {
            float percentage = stamina * 1.0f / maxStamina * 1.0f;
            StaminaBar.transform.localScale = new Vector3(percentage, StaminaBar.transform.localScale.y, StaminaBar.transform.localScale.z);
        }
    }

    void SetRechargingBlock(bool val)
    {
        rechargingBlock = val;

        if (StaminaBar != null)
        {
            Image bar = StaminaBar.GetComponent<Image>();
            if (bar != null)
            {
                if (rechargingBlock)
                {
                    bar.CrossFadeAlpha(0.5f, 0f, true);
                }
                else
                {
                    bar.CrossFadeAlpha(1f, 0f, true);
                }
            }
        }
    }
}
