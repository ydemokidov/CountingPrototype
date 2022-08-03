using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ENCAPSULATION
    public List<GameObject> pooledTargets;
    public GameObject[] targetsToPool;

    public GameObject markerArrow;
    public List<GameObject> pooledMarkers;
    public int amountToPool;
    private bool difficultySet = false;

    private int score;
    [SerializeField] private float xRange;
    [SerializeField] private float spawnInterval;
    [SerializeField] private float markerInterval;
    [SerializeField] private TMP_Dropdown difficulty;
    [SerializeField] private int targetsLeft;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI leftText;
    [SerializeField] private TMP_InputField playerNameInput;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private bool gameReady;
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject smashSoundObject;
    [SerializeField] private AudioClip[] backgroundClips;
    [SerializeField] private AudioSource backgroundMusic;

    PlayerSession session = new PlayerSession();

    void Start()
    {
        pooledTargets = new List<GameObject>();
        pooledMarkers = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            int index = UnityEngine.Random.Range(0, targetsToPool.Length);
            tmp = Instantiate(targetsToPool[index],transform.position,targetsToPool[index].transform.rotation);
            tmp.SetActive(false);
            pooledTargets.Add(tmp);

            tmp = Instantiate(markerArrow, transform.position, markerArrow.transform.rotation);
            tmp.SetActive(false);
            pooledMarkers.Add(tmp);
        }
    }

    public void StartGame()
    {
        session.playerName = playerNameInput.text;
        PlayBackgroundMusic();
        gameOverScreen.SetActive(false);
        titleScreen.SetActive(false);
        gameReady = true;

        if (!difficultySet)
        {
            spawnInterval = spawnInterval / (difficulty.value + 1);
            markerInterval = markerInterval / (difficulty.value + 1);
            difficultySet = true;
        }

        StartCoroutine(SpawnTarget(spawnInterval,markerInterval));

        ResetUILabels();
    }

    private void ResetUILabels()
    {
        score = 0;
        targetsLeft = 10;
        AddScore(score);
        leftText.SetText("Left: " + targetsLeft);
    }

    public bool IsGameReady()
    {
        return gameReady;
    }

    IEnumerator SpawnTarget(float spawnInterval, float markerInterval)
    {
        yield return new WaitForSeconds(spawnInterval);
        if (gameReady)
        {
            GameObject marker = GetPooledObject(pooledMarkers);

            Vector3 spawnPosition = GetSpawnPosition();

            marker.transform.position =spawnPosition+new Vector3(0,-2,0);
            marker.SetActive(true);
            yield return new WaitForSeconds(markerInterval);
            marker.SetActive(false);

            GameObject target = GetPooledObject(pooledTargets);
            target.transform.position = spawnPosition;
            target.SetActive(true);
            StartCoroutine(SpawnTarget(spawnInterval,markerInterval));
        }
    }

    public GameObject GetPooledObject(List<GameObject> pooledObjects)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

    public Vector3 GetSpawnPosition()
    {
        return new Vector3(UnityEngine.Random.Range(-xRange, xRange), transform.position.y, 0);
    }

    public void DestroyTarget(GameObject target)
    {
        smashSoundObject.GetComponent<AudioSource>().Play();
        target.GetComponent<Target>().DeactivateTarget();
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.SetText("Score: " + score);
    }

    public void TargetGrounded()
    {
        if (gameReady)
        {
            smashSoundObject.GetComponent<AudioSource>().Play();
            if (targetsLeft == 0)
            {
                GameOver();
                return;
            }
            targetsLeft -= 1;
            leftText.SetText("Left: " + targetsLeft);
        }
    }

    private void GameOver()
    {
        if (gameReady)
        {
            session.score = score;
            HighScoreRecord currentHighScoreRecord = session.GetCurrentHighScoreRecord();
            session.UpdateHighScoreRecordsFromFile();
            session.AddNewHighScore(currentHighScoreRecord);
            highScoreText.text = GetHighScoreText(session.GetHighScoreRecords());

            backgroundMusic.Stop();
            gameReady = false;
            gameOverScreen.SetActive(true);
        }
    }

    private void PlayBackgroundMusic()
    {
        int index = UnityEngine.Random.Range(0, backgroundClips.Length);
        backgroundMusic.clip = backgroundClips[index];
        backgroundMusic.Play();
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
