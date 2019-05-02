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

    private IEnumerator PlayOutro(PlayerControl player)
    {
        if (OutroScreen != null)
        {
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
}
