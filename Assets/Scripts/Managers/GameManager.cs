using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    Win,
    Lose,
    GameOver,
    LevelComplete
}
public class GameManager : Singleton<GameManager>
{
    public GameState CurrentState { get; private set; } = GameState.MainMenu;
    [Header("Settings")]
    [SerializeField] private float gameTimeLimit = 180f; // in seconds
    [SerializeField] private int targetScore = 100;

    public int CurrentLevel { get; set; } = 1;
    public int GravesLeft { get; set; }

    [Header("References")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject gameOverPanel;


    private float timeLeft;
    private int currentScore = 0;


    private void Start()
    {
        Grave.OnGraveSpawned += HandleGraveSpawned;
        Grave.OnGraveCleared += HandleGraveCleared;
        ChangeState(GameState.MainMenu);
        timeLeft = gameTimeLimit;
    }

    void Update()
    {
        if (CurrentState != GameState.Playing) return;


        timeLeft -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.CeilToInt(timeLeft).ToString();
        if (timeLeft <= 0)
        {
            ChangeState(GameState.GameOver);
        }
    }

    [ContextMenu("Start Game")]
    public void StartGame()
    {
        timeLeft = gameTimeLimit;
        currentScore = 0;
        ChangeState(GameState.Playing);
    }

    public void PauseGame()
    {
        if (CurrentState == GameState.Playing)
        {
            ChangeState(GameState.Paused);
        }
        else if (CurrentState == GameState.Paused)
        {
            ChangeState(GameState.Playing);
        }
    }

    public void EndGame()
    {
        Debug.Log("GameManager: Game Over");
        ChangeState(GameState.GameOver);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMenu()
    {
        ChangeState(GameState.MainMenu);
    }


    public void AddScore(int amount)
    {
        currentScore += amount;
        if (currentScore >= targetScore)
        {
            ChangeState(GameState.LevelComplete);
            return;
        }
        UpdateScore();
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
        Debug.Log("GameManager: Game State changed to: " + CurrentState);
        // Handle state-specific logic here if needed
        switch (CurrentState)
        {
            case GameState.Playing:
                Time.timeScale = 1f;
                break;
            default:
                Time.timeScale = 0f;
                break;
        }

        ShowPanel(CurrentState);
        UpdateScore();
    }

    private void ShowPanel(GameState state)
    {
        menuPanel.SetActive(state == GameState.MainMenu);
        gamePanel.SetActive(state == GameState.Playing);
        pausePanel.SetActive(state == GameState.Paused);
        winPanel.SetActive(state == GameState.Win);
        losePanel.SetActive(state == GameState.Lose);
        gameOverPanel.SetActive(state == GameState.GameOver);
    }

    private void UpdateScore()
    {
        scoreText.text = "Score: " + currentScore.ToString() + "/" + targetScore.ToString();
    }

    private void HandleGraveSpawned(Grave grave)
    {
        GravesLeft++;
        Debug.Log("Grave spawned. Graves left: " + GravesLeft);
    }

    private void HandleGraveCleared(Grave grave)
    {
        GravesLeft--;
        Debug.Log("Grave cleared. Graves left: " + GravesLeft);
        if (GravesLeft <= 0 && CurrentState == GameState.Playing)
        {
            ChangeState(GameState.Win);
        }
    }
}
