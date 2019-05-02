using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    private List<HealthSystem> players = new List<HealthSystem>();

    public GameObject IntroScreen;
    public GameObject OutroScreen;
    public GameObject BlackScreen;
    public AudioSource bgMusic;
    private Image black;

    private bool isGameActive = false;

    void Start()
    {
        players.AddRange(GameObject.FindObjectsOfType<HealthSystem>());
        Debug.Assert(players.Count != 0, "No players were found!");

        if (BlackScreen != null)
        {
            BlackScreen.SetActive(true);
            black = BlackScreen.GetComponentInChildren<Image>();
            if (black != null)
            {
                black.CrossFadeAlpha(1, 0, false);
                black.CrossFadeAlpha(0, 1, false);
            }
        }

        StartCoroutine(PlayIntro());
    }
    
    public bool IsGameActive()
    {
        return isGameActive;
    }

    public void UnregisterPlayerAfterDeath(HealthSystem p)
    {
        foreach(HealthSystem h in players)
        {
            if (h == p)
            {
                players.Remove(h);
                break;
            }
        }

        if (players.Count == 1 && players[0] != null)
        {
            PlayerControl pc = players[0].GetComponent<PlayerControl>();
            if (pc != null)
            {
                StartCoroutine("PlayOutro", pc);
            }
        }
    }


    private IEnumerator PlayIntro()
    {
        if (IntroScreen != null)
        {
            if (bgMusic != null)
            {
                IEnumerator fadeSound = FadeIn(bgMusic, 3);
                StartCoroutine(fadeSound);
            }
            IntroScreen.SetActive(true);
            TextMeshProUGUI introText = IntroScreen.GetComponentInChildren<TextMeshProUGUI>();
            if (introText != null)
            {
                introText.CrossFadeAlpha(0, 0, false);
                introText.CrossFadeAlpha(1, 1, false);

                foreach (HealthSystem player in players)
                {
                    PlayerControl pc = player.GetComponent<PlayerControl>();
                    if (pc != null)
                    {
                        pc.TriggerAnimation(PlayerAnimation.Block);
                    }
                }

                yield return new WaitForSeconds(2);
                introText.CrossFadeAlpha(0, 1, false);
                yield return new WaitForSeconds(1);
            }
            isGameActive = true;
            IntroScreen.SetActive(false);
        }
    }

    private IEnumerator PlayOutro(PlayerControl player)
    {
        if (OutroScreen != null)
        {
            if (bgMusic != null)
            {
                IEnumerator fadeSound = FadeOut(bgMusic, 5);
                StartCoroutine(fadeSound);
            }
            OutroScreen.SetActive(true);
            TextMeshProUGUI outroText = OutroScreen.GetComponentInChildren<TextMeshProUGUI>();
            if (outroText != null && player != null)
            {
                outroText.text = "Player " + player.playernumber + " won!";

                outroText.CrossFadeAlpha(0, 0, false);
                outroText.CrossFadeAlpha(1, 1, false);
                isGameActive = false;

                player.TriggerAnimation(PlayerAnimation.Taunt);

                yield return new WaitForSeconds(2);
                outroText.CrossFadeAlpha(0, 1, false);
                yield return new WaitForSeconds(1);
            }
            OutroScreen.SetActive(false);
            if (black != null)
            {
                black.CrossFadeAlpha(1, 2, false);
            }
            yield return new WaitForSeconds(2);

            SceneManager.LoadScene(0);
        }
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
