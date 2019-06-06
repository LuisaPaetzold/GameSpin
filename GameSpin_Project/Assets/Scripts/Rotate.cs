﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rotate : MonoBehaviour
{
    public float RotationSpeed = 10;

    public GameObject IntroScroll;
    public TextMeshProUGUI GameTitle;
    private bool introActiveControllerInput;
    public float introDelayTime = 0.3f;
    public AudioSource bgMusic;
    public AudioClip confirm;

    private bool introStarted;
    private bool gameStarted;

    void Start()
    {
        if (IntroScroll != null)
        {
            IntroScroll.SetActive(false);
        }
        if (bgMusic != null)
        {
            IEnumerator fadeSound = FadeIn(bgMusic, 3);
            StartCoroutine(fadeSound);
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
                if (!gameStarted)
                {
                    StartGame();
                }
            }
            else
            {
                if (!introStarted)
                {
                    StartIntro();
                }
            }
        }
    }

    void LateUpdate()
    {
        transform.Rotate(0, RotationSpeed * Time.deltaTime, 0);
    }

    public void StartGame()
    {
        if (bgMusic != null)
        {
            bgMusic.PlayOneShot(confirm);
            IEnumerator fadeSound = FadeOut(bgMusic, 3);
            StartCoroutine(fadeSound);
        }
        gameStarted = true;
        SceneManager.LoadScene(1);
    }

    public void StartIntro()
    {
        introStarted = true;
        StartCoroutine(SetIntroAsActiveWithDelay());
    }

    IEnumerator SetIntroAsActiveWithDelay()
    {
        // used for controller input: inserts small delay so that hitting the button doesn't immediately start the game after loading the intro
        if (bgMusic != null)
        {
            bgMusic.PlayOneShot(confirm);
        }

        if (IntroScroll != null)
        {
            IntroScroll.SetActive(true);
        }

        if (GameTitle != null)
        {
            GameTitle.CrossFadeAlpha(0, 0.5f, false);
        }

        yield return new WaitForSeconds(introDelayTime);
        introActiveControllerInput = true;
    }

    public IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    public IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        float finalVolume = audioSource.volume;

        audioSource.volume = 0;

        while (audioSource.volume < finalVolume)
        {
            audioSource.volume += finalVolume * Time.deltaTime / FadeTime;

            yield return null;
        }
    }
}
