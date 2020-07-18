using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameoverScreenController : MonoBehaviour
{
    public UnityEngine.UI.Text newHighScore;
    public UnityEngine.UI.Text HighScoreText;
    public UnityEngine.UI.Text highScoreLabel;
    public UnityEngine.UI.Text FinalScoreText;
    public UnityEngine.UI.Text helpText;
    public UnityEngine.UI.Text overallHighScoreText;
    public Canvas endGameScores;

    public UnityEngine.UI.Button ReturnToMenuButton;
    public UnityEngine.UI.Button TryAgainButton;
    public UnityEngine.UI.Button NextLevelButton;
    public GameObject NextLevelButtonObject;

    public int finalScore;
    public int highScore;
    public int overallScore;
    public int overallHighScore;
    public int scoreGoal;
    public bool easymode;

    private LevelManager levelManager;
    private int currentLevel;

    private void Start()
    {
        endGameScores = this.GetComponent<Canvas>();
        levelManager = GameObject.Find("SceneManager").GetComponent<LevelManager>();        
    }

    void Awake()
    {
        // DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(finalScore < scoreGoal)
        {
            NextLevelButton.interactable = false;
        }
        else
        {
            NextLevelButton.interactable = true;
        }
    }

    public void toggleCanvas()
    {
        Debug.Log("toggle canvas");
        currentLevel = levelManager.getCurrentLevel();
        if(currentLevel >= 3)
        {
            NextLevelButtonObject.SetActive(false);
            helpText.enabled = false;
        }
        endGameScores.enabled = !endGameScores.enabled;
    }

    public void setFinalScore(int scoreV)
    {
        finalScore = scoreV;
        FinalScoreText.text = finalScore.ToString();
        currentLevel = levelManager.getCurrentLevel();
        if (finalScore < scoreGoal)
        {
            helpText.text = "Only " + (scoreGoal - finalScore) + " more to unlock level " + (currentLevel + 1);
        }
        else
        {
            helpText.text = "You've unlocked the next level!";
        }
    }

    public int getFinalScore()
    {
        return finalScore;
    }

    public void setHighScore(int scoreV)
    {
        highScore = scoreV;
        currentLevel = levelManager.getCurrentLevel();
        PlayerPrefs.SetInt("Level" + currentLevel.ToString(), highScore);
        highScoreLabel.text = "Level " + currentLevel + " High Score";
        HighScoreText.text = highScore.ToString();
    }

    public int getHighScore()
    {
        currentLevel = levelManager.getCurrentLevel();
        highScore = PlayerPrefs.GetInt("Level" + currentLevel.ToString());
        return highScore;
    }

    public void setOverallScore(int scoreV)
    {
        overallScore += scoreV;
        overallHighScore = PlayerPrefs.GetInt("OverallHighScore");
        overallHighScoreText.text = overallHighScore.ToString();

    }

    public int getOverallScore()
    {
        return overallScore;
    }

    public int getOverallHighScore()
    {
        overallHighScore = PlayerPrefs.GetInt("OverallHighScore");
        return overallHighScore;
    }

    public void ReturnToMenu()
    {
        toggleCanvas();
        Debug.Log("Returning to menu");
        overallHighScore = PlayerPrefs.GetInt("OverallHighScore");
        if(overallScore > overallHighScore)
        {
            overallHighScore = overallScore;
            PlayerPrefs.SetInt("OverallHighScore", overallHighScore);
            overallHighScoreText.text = overallHighScore.ToString();
        }
        overallScore = 0;
        levelManager.MainMenu();
    }

    public void TryAgain()
    {
        Debug.Log("Try again");
        toggleCanvas();
        overallScore -= finalScore;
        levelManager.ReloadLevel();
    }

    public void NextLevel()
    {
        toggleCanvas();
        Debug.Log("Going to next level");
        overallHighScore = PlayerPrefs.GetInt("OverallHighScore");
        if (overallScore > overallHighScore)
        {
            overallHighScore = overallScore;
            PlayerPrefs.SetInt("OverallHighScore", overallHighScore);
            overallHighScoreText.text = overallHighScore.ToString();
        }
        levelManager.GoToLevel();
    }

    public void SetScoreGoal(int goal)
    {
        if(levelManager.easymode)
        {
            scoreGoal = goal / 2;
        }
        else
        {
            scoreGoal = goal;
        }
    }
}
