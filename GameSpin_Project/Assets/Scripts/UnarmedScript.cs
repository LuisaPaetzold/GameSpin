using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnarmedScript : MonoBehaviour
{
    private bool attackInProgress;

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
