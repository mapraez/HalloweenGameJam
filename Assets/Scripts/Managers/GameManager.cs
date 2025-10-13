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
        timeLeft = gameTimeLimit;
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

    [ContextMenu("Start Game")]
    public void StartGame()
    {
        Debug.Log("GameManager: Starting Game");
        timeLeft = gameTimeLimit;
        currentScore = 0;
        ChangeState(GameState.Playing);
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
            ChangeState(GameState.Win);
        }
    }
}
