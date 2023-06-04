using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    private void Start()
    {
        GameData.Health = 3;
        GameData.Score = 0;
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void GoToEasyGameScene()
    {
        if (GameData.Volume <= 0.1f)
        {
            WriteToFile("Time", "Score", "Lives (No Music)");
        }
        else
        {
            WriteToFile("Time", "Score", "Lives (Music)");
        }

        
        GameData.CurrentDiff = GameDiff.Easy;
        SceneManager.LoadScene("Main");
    }

    public void GoToMediumGameScene()
    {
        if (GameData.Volume <= 0.1f)
        {
            WriteToFile("Time", "Score", "Lives (No Music)");
        }
        else
        {
            WriteToFile("Time", "Score", "Lives (Music)");
        }
        GameData.CurrentDiff = GameDiff.Medium;
        SceneManager.LoadScene("Main");
    }

    public void GoToHardGameScene()
    {
        if (GameData.Volume <= 0.1f)
        {
            WriteToFile("Time", "Score", "Lives (No Music)");
        }
        else
        {
            WriteToFile("Time", "Score", "Lives (Music)");
        }

        GameData.CurrentDiff = GameDiff.Hard;
        SceneManager.LoadScene("Main");
    }

    public void GoToHighscores()
    {
        SceneManager.LoadScene("Highscores");
    }

    public void GoToTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void GoToDiffSelect()
    {
        GameData.Volume = GameObject.Find("Slider").GetComponent<Slider>().value;
        SceneManager.LoadScene("DifficultySelect");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void WriteToFile(string time, string score, string lives)
    {
        string filePath = Application.persistentDataPath + "/data.csv";

        using (StreamWriter writer = File.AppendText(filePath))
        {
            writer.WriteLine(time + "," + score + "," + lives);
        }
    }
}
