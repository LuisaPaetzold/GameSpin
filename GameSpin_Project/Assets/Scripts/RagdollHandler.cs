using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollHandler : MonoBehaviour
{

    private List<Rigidbody> bodyparts;


    // Start is called before the first frame update
    void Start()
    {
        bodyparts = new List<Rigidbody>();
        foreach(Rigidbody rigid in GameObject.FindObjectsOfType<Rigidbody>())
        {
            bodyparts.Add(rigid);
        }

        toggleRigidbody(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleRigidbody( bool val)
    {
        foreach (Rigidbody bodypart in bodyparts)
        {
            bodypart.isKinematic = val;
        }

    }


}
