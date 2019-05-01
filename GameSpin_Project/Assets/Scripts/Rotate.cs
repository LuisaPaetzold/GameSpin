using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rotate : MonoBehaviour
{
    public float RotationSpeed = 10;

    public GameObject IntroScroll;
    private bool introActiveControllerInput;
    public float introDelayTime = 1;

    void Start()
    {
        if (IntroScroll != null)
        {
            IntroScroll.SetActive(false);
        }
    }

    void Update()
    {
        float hit1 = Input.GetAxis("Fire1_P" + 1);
        float hit2 = Input.GetAxis("Fire1_P" + 2);

        if (Input.GetKeyDown("s"))
        {
            StartGame();
        }
        if (hit1 != 0 || hit2 != 0)
        {
            if (introActiveControllerInput)
            {
                StartGame();
            }
            else
            {
                StartIntro();
            }
        }
    }

    void LateUpdate()
    {
        transform.Rotate(0, RotationSpeed * Time.deltaTime, 0);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void StartIntro()
    {
        StartCoroutine(SetIntroAsActiveWithDelay());
    }

    IEnumerator SetIntroAsActiveWithDelay()
    {
        // used for controller input: inserts small delay so that hitting the button doesn't immediately start the game after loading the intro
        if (IntroScroll != null)
        {
            IntroScroll.SetActive(true);
        }
        yield return new WaitForSeconds(introDelayTime);
        introActiveControllerInput = true;
    }
}
