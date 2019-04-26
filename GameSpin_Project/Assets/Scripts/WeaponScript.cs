using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public int damage;
    private bool attackInProgress;


    public int GetDamage()
    {
        return damage;
    }

    public void InitiateAttack(float duration)
    {
        StartCoroutine("ProcessAttack", duration);
    }

    private IEnumerator ProcessAttack(float duration)
    {
        // let weapon know that an attack is in progress

        attackInProgress = true;

        yield return new WaitForSeconds(duration);

        attackInProgress = false;
    }

    public bool IsAttacking()
    {
        return attackInProgress;
    }
}
