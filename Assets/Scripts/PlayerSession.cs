using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class PlayerSession
{
    public string playerName;
    public int score;

    private int highScoreTableSize = 5;
    private string filename = "highscore.json";
    public List<HighScoreRecord> highScoreRecords = new List<HighScoreRecord>();

    public void UpdateHighScoreRecordsFromFile()
    {
        string datapath = Application.persistentDataPath + "/" + filename;
        if (File.Exists(datapath))
        {
            string jsonString = File.ReadAllText(datapath);
            HighScoreList highScoreList = JsonUtility.FromJson<HighScoreList>(jsonString);
            highScoreRecords = highScoreList.records;
        }
    }

    public List<HighScoreRecord> GetHighScoreRecords()
    {
        return highScoreRecords;
    }

    public void AddNewHighScore(HighScoreRecord newRecord)
    {
        bool modified = false;
        if (highScoreRecords.Count == 0)
        {
            highScoreRecords.Add(newRecord);
            modified = true;
        }
        else
        {

            int limit = highScoreRecords.Count < highScoreTableSize ? highScoreRecords.Count : highScoreTableSize;

            for (int i = 0; i < limit; i++)
            {
                if (newRecord.highScore > highScoreRecords[i].highScore)
                {
                    highScoreRecords.Insert(i, newRecord);

                    if (highScoreRecords.Count > highScoreTableSize)
                    {
                        highScoreRecords.RemoveAt(highScoreTableSize);
                    }
                    modified = true;
                    break;
                }
            }
        }

        if (modified)
        {
            SaveHighScoreRecordsToFile();
        }

    }

    public void SaveHighScoreRecordsToFile()
    {
        string datapath = Application.persistentDataPath + "/" + filename;

        HighScoreList highScoreList = new HighScoreList();
        highScoreList.records = GetHighScoreRecords();

        string json = JsonUtility.ToJson(highScoreList);
        File.WriteAllText(datapath, json);
    }

    public HighScoreRecord GetCurrentHighScoreRecord()
    {
        HighScoreRecord result = new HighScoreRecord();
        result.dateTime = DateTime.Now;
        result.playerName = playerName;
        result.highScore = score;

        return result;
    }


}
