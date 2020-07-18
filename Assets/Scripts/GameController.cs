using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
  public UnityEngine.UI.Text scoreUIElement;
    public UnityEngine.UI.Text overallScoreElement;
  // public UnityEngine.UI.Text newHighScore;
  // public UnityEngine.UI.Text HighScoreText;
  // public UnityEngine.UI.Text FinalScoreText;
  public Canvas endGameScores;
  public Canvas UI;
  public int score;
  public int overallScore;
  private int highscore;

  public float scoreTimeRate = 0.5f;

  public GameObject player;
  public GameObject sceneManager;
  private LevelManager levelManager;
  public GameoverScreenController gameoverScreen;

  public GameObject banana;
  public List<GameObject> availableBananas;
  public int minimumAvaliableBananas = 4;
  public List<Vector3> availableSpawnPositions;

  public int scoreGoal = 300;

  void Start()
  {
    player = GameObject.Find("player");
    InvokeRepeating("AddScoreTime", .5f, scoreTimeRate);

    availableBananas = new List<GameObject>(GameObject.FindGameObjectsWithTag("banana"));
    Debug.Log("Count: " + GameObject.FindGameObjectsWithTag("banana").Length);
    availableSpawnPositions = new List<Vector3>();
    endGameScores = GameObject.Find("EndGameCanvasHolder").GetComponent<CanvasContainer>().getEndGamePanel();
    gameoverScreen = endGameScores.GetComponent<GameoverScreenController>();
    sceneManager = GameObject.Find("SceneManager");
    levelManager = sceneManager.GetComponent<LevelManager>();
    overallScore += gameoverScreen.getOverallScore();
    
    if (gameoverScreen != null) gameoverScreen.SetScoreGoal(scoreGoal);
  }

  void Update()
  {
    // Display Score
    scoreUIElement.text = "Score: " + score;
    overallScoreElement.text = "Overall Score: " + overallScore;

    if (availableBananas.Count < minimumAvaliableBananas && availableSpawnPositions.Count > 0)
    {
      SpawnBanana();
    }
  }

  void SpawnBanana()
  {
    int randomSpawnIndex = Random.Range(0, availableSpawnPositions.Count - 1);
    Vector3 randomSpawnPos = availableSpawnPositions[randomSpawnIndex];

    GameObject newBanana = Instantiate(banana, randomSpawnPos, Quaternion.identity);

    availableSpawnPositions.Remove(randomSpawnPos);
    availableBananas.Add(newBanana);
  }

  public void AddScore(int score)
  {
    this.score += score;
        overallScore += score;
  }

  void AddScoreTime()
  {
    score++;
        overallScore++;
  }

  public void GameOver()
  {
    // Game ends here, stop inputs and show endgame UI
    CancelInvoke("AddScoreTime");
        Debug.Log("GameOver");
    gameoverScreen.toggleCanvas();
    gameoverScreen.setFinalScore(score);
    gameoverScreen.setHighScore(score);
    gameoverScreen.setOverallScore(score);
    // levelManager.MainMenu();
  }

  /*public void returnToMenu()
  {
    levelManager.MainMenu();
  }

  public void tryAgain()
  {
    levelManager.GoToLevel();
  }*/
}
