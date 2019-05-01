using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rotate : MonoBehaviour
{
    public float RotationSpeed = 10;

    public GameObject IntroScroll;

    void Start()
    {
        if (IntroScroll != null)
        {
            IntroScroll.SetActive(false);
        }
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

    public void StartIntro()
    {
        if (IntroScroll != null)
        {
            IntroScroll.SetActive(true);
        }
    }

}
