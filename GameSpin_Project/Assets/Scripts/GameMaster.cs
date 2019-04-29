using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    private List<HealthSystem> players = new List<HealthSystem>();

    public GameObject IntroScreen;
    public GameObject OutroScreen;
    public GameObject BlackScreen;
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

        if (players.Count == 1)
        {
            Debug.Log("WIN");
            StartCoroutine(PlayOutro());
        }
    }


    private IEnumerator PlayIntro()
    {
        if (IntroScreen != null)
        {
            IntroScreen.SetActive(true);
            TextMeshProUGUI introText = IntroScreen.GetComponentInChildren<TextMeshProUGUI>();
            if (introText != null)
            {
                introText.CrossFadeAlpha(0, 0, false);
                introText.CrossFadeAlpha(1, 1, false);
                yield return new WaitForSeconds(2);
                introText.CrossFadeAlpha(0, 1, false);
                yield return new WaitForSeconds(1);
            }
            isGameActive = true;
            IntroScreen.SetActive(false);
        }
    }

    private IEnumerator PlayOutro()
    {
        if (OutroScreen)
        {
            OutroScreen.SetActive(true);
            TextMeshProUGUI outroText = OutroScreen.GetComponentInChildren<TextMeshProUGUI>();
            if (outroText != null)
            {
                outroText.CrossFadeAlpha(0, 0, false);
                outroText.CrossFadeAlpha(1, 1, false);
                yield return new WaitForSeconds(2);
                outroText.CrossFadeAlpha(0, 1, false);
                yield return new WaitForSeconds(1);
            }
            isGameActive = false;
            OutroScreen.SetActive(false);
            if (black != null)
            {
                black.CrossFadeAlpha(1, 2, false);
            }
        }
    }
}
