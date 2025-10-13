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

    [SerializeField] public int CurrentLevel { get; set; } = 1;
    [SerializeField] public int GravesLeft { get; set; } = 0;




    private float timeLeft;
    private int currentScore = 0;


    override protected void Awake()
    {
        base.Awake();
        Grave.OnGraveSpawned += HandleGraveSpawned;
        Grave.OnGraveCleared += HandleGraveCleared;
    }


    private void Start()
    {
        Debug.Log("GameManager: Starting in MainMenu state");
        ChangeState(GameState.MainMenu);
        currentScore = 0;
    }

    void Update()
    {
        if (CurrentState != GameState.Playing) return;

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0)
        {
            ChangeState(GameState.GameOver);
        }
        else
        {
            UIManager.Instance.UpdateTimer(timeLeft);
        }
    }

    [ContextMenu("Begin Level")]
    public void BeginLevel()
    {
        Debug.Log("GameManager: Starting Level " + CurrentLevel);
        ChangeState(GameState.Playing);
        timeLeft = gameTimeLimit;
    }

    public void TogglePause()
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
    
    public void LoadNextLevel()
    {
        int nextLevelIndex = CurrentLevel + 1;
        if (nextLevelIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("GameManager: No more levels to load. You win the game!");
            WinGame();
            return;
        }
        LoadLevel(nextLevelIndex);

    }

    public void WinGame()
    {
        Debug.Log("GameManager: You Win!");
        ChangeState(GameState.Win);
    }

    public void EndGame()
    {
        Debug.Log("GameManager: Game Over");
        ChangeState(GameState.GameOver);
    }

    public void RestartCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
        ChangeState(GameState.MainMenu);

    }

    public void QuitToMenu()
    {
        ChangeState(GameState.MainMenu);
        // SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Debug.Log("GameManager: Exiting Game");
        Application.Quit();
    }


    public void AddScore(int amount)
    {
        currentScore += amount;
        if (currentScore >= targetScore)
        {
            ChangeState(GameState.LevelComplete);
            return;
        }
        UIManager.Instance.UpdateScore(currentScore, targetScore);
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

        UIManager.Instance.ShowPanel(CurrentState);
        UIManager.Instance.UpdateScore(currentScore, targetScore);
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
            ChangeState(GameState.LevelComplete);
        }
    }
}
