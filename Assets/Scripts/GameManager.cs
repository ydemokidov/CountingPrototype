using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ENCAPSULATION

    private bool difficultySet = false;

    private int score;
    [SerializeField] private TMP_Dropdown difficulty;
    [SerializeField] private int targetsLeft;
    [SerializeField] private SoundPlayer soundPlayer;
    [SerializeField] private UIHandler uiHandler;

    [SerializeField] private TMP_InputField playerNameInput;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private bool gameReady;

    PlayerSession session = new PlayerSession();
    [SerializeField] private TargetSpawner targetSpawner;

    public void StartGame()
    {
        session.playerName = playerNameInput.text;
        soundPlayer.PlayBackgroundMusic();
        uiHandler.DeactivateGameOverScreen();
        uiHandler.DeactivateTitleScreen();
        gameReady = true;

        if (!difficultySet)
        {
            targetSpawner.Init(difficulty.value);
            difficultySet = true;
        }

        targetSpawner.BeginSpawn();

        uiHandler.ResetUILabels(0, 10);
    }

    public bool IsGameReady()
    {
        return gameReady;
    }

    public void DestroyTarget(GameObject target)
    {
        soundPlayer.PlaySmashSound();
        target.GetComponent<Target>().DeactivateTarget();
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        uiHandler.SetScoreText(score);
    }

    public void TargetGrounded()
    {
        if (gameReady)
        {
            soundPlayer.PlaySmashSound();
            if (targetsLeft == 0)
            {
                GameOver();
                return;
            }
            targetsLeft -= 1;
            uiHandler.SetTargetsLeftText(targetsLeft);
        }
    }

    private void GameOver()
    {
        if (gameReady)
        {
            targetSpawner.StopSpawn();
            session.score = score;
            HighScoreRecord currentHighScoreRecord = session.GetCurrentHighScoreRecord();
            session.UpdateHighScoreRecordsFromFile();
            session.AddNewHighScore(currentHighScoreRecord);
            highScoreText.text = GetHighScoreText(session.GetHighScoreRecords());

            soundPlayer.StopBackgroundMusic();
            gameReady = false;
            uiHandler.ActivateGameOverScreen();
        }
    }

    private string GetHighScoreText(List<HighScoreRecord> records)
    {
        string result = "Top5:" + Environment.NewLine;
        foreach(HighScoreRecord record in records)
        {
            result += record.ToFormattedString() + Environment.NewLine;
        }
        return result;
    }

}
