using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rotate : MonoBehaviour
{
    public float RotationSpeed = 10;

    public GameObject IntroScroll;
    public GameObject Controls;
    public GameObject MainUI;
    public TextMeshProUGUI GameTitle;
    public GameObject CtrlButtonToHide;
    private bool introActiveControllerInput;
    public float introDelayTime = 0.3f;
    public AudioSource bgMusic;
    public AudioClip confirm;

    private bool introStarted;
    private bool gameStarted;
    private bool ctrlActive;

    void Start()
    {
        if (IntroScroll != null)
        {
            IntroScroll.SetActive(false);
        }
        if (Controls != null)
        {
            Controls.SetActive(false);
        }
        if (MainUI != null)
        {
            MainUI.SetActive(true);
        }
        if (bgMusic != null)
        {
            IEnumerator fadeSound = FadeIn(bgMusic, 3);
            StartCoroutine(fadeSound);
        }
    }

    void Update()
    {
        bool hit1 = Input.GetButtonDown("Fire1_P" + 1);
        bool hit2 = Input.GetButtonDown("Fire1_P" + 2);

        bool ctrl1 = Input.GetButtonDown("Jump_P" + 1);
        bool ctrl2 = Input.GetButtonDown("Jump_P" + 2);

        if (hit1 || hit2)
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

        if (ctrl1 || ctrl2)
        {
            if (ctrlActive)
            {
                HideControls();
            }
            else
            {
                DisplayControls();
            }
        }
    }

    void LateUpdate()
    {
        transform.Rotate(0, RotationSpeed * Time.deltaTime, 0);
    }

    public void StartGame()
    {
        if (ctrlActive)
        {
            return; // we cannot start the game as long as the controls are displayed
        }

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
        if (ctrlActive)
        {
            return; // we cannot start the game as long as the controls are displayed
        }

        introStarted = true;
        StartCoroutine(SetIntroAsActiveWithDelay());
    }

    public void DisplayControls()
    {
        if (introStarted)
        {
            return; // as soon as the intro started, we are no longer able to access the controls
        }

        if (MainUI != null)
        {
            MainUI.SetActive(false);
        }
        if (Controls != null)
        {
            Controls.SetActive(true);
        }
        ctrlActive = true;
    }

    public void HideControls()
    {
        if (introStarted)
        {
            return; // as soon as the intro started, we are no longer able to access the controls
        }

        if (MainUI != null)
        {
            MainUI.SetActive(true);
        }
        if (Controls != null)
        {
            Controls.SetActive(false);
        }
        ctrlActive = false;
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
        if(CtrlButtonToHide != null)
        {
            CtrlButtonToHide.SetActive(false);
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
