using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{

    public int lives;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(lives <= 0)
        {
            Destroy(gameObject, 10);
            gameObject.GetComponent<Animator>().SetBool("Death", true);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.collider.tag == "weapon")
        {
            this.lives -= collision.collider.GetComponent<WeaponScript>().damage;
            gameObject.GetComponent<Animator>().SetTrigger("isHit");
        }
    }
}
