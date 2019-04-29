using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rotate : MonoBehaviour
{
    public float RotationSpeed = 10;

    void Start()
    {

    }

    void Update()
    {
        transform.Rotate(0, RotationSpeed * Time.deltaTime, 0);

        if (Input.GetKeyDown("s"))
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

}
