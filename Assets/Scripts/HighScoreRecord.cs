using System;

[Serializable]
public class HighScoreRecord
{

    public DateTime dateTime;

    public string playerName;

    public int highScore;

    public string ToFormattedString()
    {
        return playerName + " : " + highScore + " : " + dateTime.ToString();
    }

}
