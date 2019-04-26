using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : MonoBehaviour
{
    private const float SPEED = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        float hit = Input.GetAxis("Fire1");


        if (!GameObject.FindObjectOfType<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        if (movement.sqrMagnitude != 0)
        {
            gameObject.GetComponent<Animator>().SetBool("Move", true);
            transform.rotation = Quaternion.LookRotation(movement);
            transform.Translate(movement * Time.deltaTime * SPEED, Space.World);
        }
        else
        {
                gameObject.GetComponent<Animator>().SetBool("Move", false);
        }


       
        if (hit != 0)
        {
                gameObject.GetComponent<Animator>().SetTrigger("Hit");
        }
        }




    }
}
