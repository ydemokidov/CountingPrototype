using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI leftText;
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject gameOverScreen;

    public void DeactivateTitleScreen()
    {
        titleScreen.SetActive(false);
    }

    public void DeactivateGameOverScreen()
    {
        gameOverScreen.SetActive(false);
    }

    public void ActivateGameOverScreen()
    {
        gameOverScreen.SetActive(true);
    }

    public void ResetUILabels(int score, int targetsLeft)
    {
        SetScoreText(0);
        targetsLeft = 10;
        leftText.SetText("Left: " + targetsLeft);
    }

    public void SetScoreText(int score)
    {
        scoreText.SetText("Score: " + score);
    }

    public void SetTargetsLeftText(int targetsLeft)
    {
        leftText.SetText("Left: " + targetsLeft);
    }
}
