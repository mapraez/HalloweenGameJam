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
    LevelComplete,
    LevelMenu
}



public class GameManager : Singleton<GameManager>
{
    public GameState CurrentGameState { get; private set; } = GameState.MainMenu;
    [Header("Settings")]
    [SerializeField] private float gameTimeLimit = 180f; // in seconds
    [SerializeField] private int targetScore = 100;

    [SerializeField] public int CurrentLevel { get; set; } = 1;
    [SerializeField] public int GravesLeft { get; set; } = 0;


    private int scoreAtLevelStart;
    private float timeLeftAtLevelStart;

    private float timeLeft;
    public int CurrentScore { get; set; } = 0;
    public string PlayerName { get; set; } = "Player1";


    override protected void Awake()
    {
        base.Awake();
        Grave.OnGraveSpawned += HandleGraveSpawned;
        Grave.OnGraveCleared += HandleGraveCleared;
    }


    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            Debug.Log("GameManager: Starting in Sample scene, going to LevelMenu state");
            ChangeState(GameState.LevelMenu);
            timeLeft = gameTimeLimit;
            return;
        }
        Debug.Log("GameManager: Starting in MainMenu state");
        ChangeState(GameState.MainMenu);

    }

    void Update()
    {
        if (CurrentGameState != GameState.Playing) return;

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0)
        {
            ChangeState(GameState.Lose);
        }
        else
        {
            UIManager.Instance.UpdateTimer(timeLeft);
        }
    }

    public void AddScore(int amount)
    {
        CurrentScore += amount;
        if (CurrentScore >= targetScore)
        {
            ChangeState(GameState.LevelComplete);
            return;
        }
        UIManager.Instance.UpdateScore(CurrentScore, targetScore);
    }

    public void TogglePause()
    {
        if (CurrentGameState == GameState.Playing)
        {
            ChangeState(GameState.Paused);
        }
        else if (CurrentGameState == GameState.Paused)
        {
            ChangeState(GameState.Playing);
        }
    }

    [ContextMenu("Start Game")]
    public void StartGame()
    {
        SceneManager.LoadScene("Level_01");
        ChangeState(GameState.LevelMenu);
        timeLeft = gameTimeLimit;
        CurrentScore = 0;
    }


    [ContextMenu("Start Current Level")]
    public void StartCurrentLevel()
    {
        Debug.Log("GameManager: Starting Level " + CurrentLevel);
        scoreAtLevelStart = CurrentScore;
        timeLeftAtLevelStart = timeLeft;
        ChangeState(GameState.Playing);
    }

    [ContextMenu("Restart Current Level")]
    public void RestartCurrentLevel()
    {
        CurrentScore = scoreAtLevelStart;
        timeLeft = timeLeftAtLevelStart;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    [ContextMenu("Load Next Level")]
    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextLevelIndex = currentSceneIndex + 1;
        if (nextLevelIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("GameManager: No more levels to load. You win the game!");
            WinGame();
            return;
        }
        LoadLevel(nextLevelIndex);

    }


    public void LoadLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError("GameManager: Invalid level index: " + levelIndex);
            return;
        }
        CurrentLevel = levelIndex;
        SceneManager.LoadScene(levelIndex);
        ChangeState(GameState.LevelMenu);

    }

    public void QuitToMenu()
    {
        CurrentScore = 0;
        timeLeft = gameTimeLimit;
        Destroy(PlayerController.Instance.gameObject);
        Debug.Log("GameManager: Quitting to Main Menu");
        ChangeState(GameState.MainMenu);
    }

    public void ExitGame()
    {
        Debug.Log("GameManager: Exiting Game");
        Application.Quit();
    }

    [ContextMenu("Win Game")]
    public void WinGame()
    {
        Debug.Log("GameManager: You Win!");
        CurrentScore += Mathf.FloorToInt(timeLeft); // Bonus for remaining time

        ChangeState(GameState.Win);
    }

    [ContextMenu("Lose Game")]
    public void LoseGame()
    {
        Debug.Log("GameManager: You Lose!");
        ChangeState(GameState.Lose);
    }

    [ContextMenu("End Game")]
    public void EndGame()
    {
        Debug.Log("GameManager: Game Over");
        ChangeState(GameState.GameOver);
    }

    public void ChangeState(GameState newState)
    {
        CurrentGameState = newState;
        Debug.Log("GameManager: Game State changed to: " + CurrentGameState);
        // Handle state-specific logic here if needed
        Time.timeScale = 0f;
        switch (CurrentGameState)
        {
            case GameState.Playing:
                Time.timeScale = 1f;
                break;
            case GameState.Win:
            case GameState.Lose:
            case GameState.GameOver:
                SceneManager.LoadScene("GameOverScene");
                break;
            case GameState.MainMenu:
                if (SceneManager.GetActiveScene().name != "MainMenu")
                {
                    SceneManager.LoadScene("MainMenu");
                }
                break;
            default:
                break;
        }
        UIManager.Instance.UpdateUI(CurrentGameState, timeLeft, CurrentScore, targetScore);

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
        if (GravesLeft <= 0 && CurrentGameState == GameState.Playing)
        {
            ChangeState(GameState.LevelComplete);
        }
    }


    void OnDestroy()
    {
        Grave.OnGraveSpawned -= HandleGraveSpawned;
        Grave.OnGraveCleared -= HandleGraveCleared;
    }
}
