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
    LevelMenu,
    Intro
}



public class GameManager : Singleton<GameManager>
{
    public GameState CurrentGameState { get; private set; } = GameState.MainMenu;


    [Header("Settings")]
    [SerializeField] private float gameTimeLimit = 180f; // in seconds
    [SerializeField] private int targetScore = 100;

    [SerializeField] public int CurrentLevel { get; set; } = 1;
    [SerializeField] public int GravesLeft { get; set; } = 0;

    [SerializeField] private GameObject playerSpawnEffectPrefab;


    private int scoreAtLevelStart;
    private float timeLeftAtLevelStart;
    private int gravesAtLevelStart;
    private int playerHealthAtLevelStart;

    private float timeLeft;
    public int CurrentScore { get; set; } = 0;
    public int FinalScore { get; set; } = 0;
    public string PlayerName { get; set; } = "Player1";

    readonly public string FirstLevelName = "Level_01";
    readonly public string MainMenuSceneName = "MainMenu";
    readonly public string GameOverSceneName = "GameOverScene";
    readonly public string TestSceneName = "TestScene";
    

    override protected void Awake()
    {
        base.Awake();
        Grave.OnGraveSpawned += HandleGraveSpawned;
        Grave.OnGraveCleared += HandleGraveCleared;
    }


    private void Start()
    {
        if (SceneManager.GetActiveScene().name == TestSceneName)
        {
            Debug.Log("GameManager: Starting in Test scene, going to LevelMenu state");
            ChangeState(GameState.LevelMenu);
            timeLeft = gameTimeLimit;
            return;
        }
        GoToMainMenu();

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
            Debug.Log("GameManager: Target score reached!");
            PlayerController.Instance.HealPlayer(1);
            targetScore += 100; // Increase target for next time
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
        SceneManager.LoadScene(FirstLevelName);
        SoundManager.Instance.PlayGameplayMusic();
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
        playerHealthAtLevelStart = PlayerController.Instance.CurrentHealth;
        ChangeState(GameState.Playing); 

        GameObject spawnEffect = Instantiate(playerSpawnEffectPrefab, PlayerController.Instance.transform.position + Vector3.up * .1f, Quaternion.identity);
        Destroy(spawnEffect, 2f); // Destroy the effect after 2 seconds
    }

    [ContextMenu("Restart Current Level")]
    public void RestartCurrentLevel()
    {
        Debug.Log("GameManager: Restarting Level " + CurrentLevel);
        CurrentScore = scoreAtLevelStart;
        timeLeft = timeLeftAtLevelStart;
        GravesLeft = 0;
        PlayerController.Instance.SetPlayerHealth(playerHealthAtLevelStart);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        ChangeState(GameState.LevelMenu);
    }

    [ContextMenu("Load Next Level")]
    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextLevelIndex = currentSceneIndex + 1;

        CurrentLevel++;
        GravesLeft = 0;

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
        SceneManager.LoadScene(levelIndex);
        ChangeState(GameState.LevelMenu);
        SoundManager.Instance.PlayGameplayMusic();

    }

    public void GoToMainMenu()
    {
        CurrentScore = 0;
        timeLeft = gameTimeLimit;
        if (PlayerController.Instance != null)
        {
            Destroy(PlayerController.Instance.gameObject);
        }
        Debug.Log("GameManager: Quitting to Main Menu");
        ChangeState(GameState.MainMenu);
        SoundManager.Instance.PlayMainMenuMusic();
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
        FinalScore = CurrentScore + Mathf.CeilToInt(timeLeft); // Bonus for remaining time

        ChangeState(GameState.Win);
        SoundManager.Instance.PlayWinMusic();
    }

    [ContextMenu("Lose Game")]
    public void LoseGame()
    {
        Debug.Log("GameManager: You Lose!");
        ChangeState(GameState.Lose);
        SoundManager.Instance.PlayGameOverMusic();
    }

    [ContextMenu("End Game")]
    public void EndGame()
    {
        Debug.Log("GameManager: Game Over");
        ChangeState(GameState.GameOver);
        SoundManager.Instance.PlayGameOverMusic();
    }

    public void ChangeState(GameState newState)
    {
        CurrentGameState = newState;
        Debug.Log("GameManager: Game State changed to: " + CurrentGameState);
        // Handle state-specific logic here if needed
        Time.timeScale = 0f;
        switch (CurrentGameState)
        {
            case GameState.MainMenu:
                Time.timeScale = 1f;
                if (SceneManager.GetActiveScene().name != MainMenuSceneName)
                {
                    SceneManager.LoadScene(MainMenuSceneName);
                }
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                break;
            case GameState.Win:
            case GameState.Lose:
            case GameState.GameOver:
                UIManager.Instance.ClearGravesText();
                SceneManager.LoadScene(GameOverSceneName);
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
        UIManager.Instance.UpdateGravesLeft(GravesLeft);
    }

    private void HandleGraveCleared(Grave grave)
    {
        GravesLeft--;
        Debug.Log("Grave cleared. Graves left: " + GravesLeft);
        UIManager.Instance.UpdateGravesLeft(GravesLeft);
        
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
