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
    GameOver
}
public class GameManager : Singleton<GameManager>
{
    public GameState CurrentState { get; private set; } = GameState.MainMenu;
    [Header("Settings")]
    [SerializeField] private float gameTimeLimit = 180f; // in seconds
    [SerializeField] private int targetScore = 100;

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
    }
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
            Time.timeScale = 0f;
        }
        else if (CurrentState == GameState.Paused)
        {
            ChangeState(GameState.Playing);
            Time.timeScale = 1f;
        }
    }

    public void EndGame()
    {
        Debug.Log("GameManager: Game Over");
        Time.timeScale = 0f;
        ChangeState(GameState.GameOver);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        ChangeState(GameState.MainMenu);
    }


    public void AddScore(int amount)
    {
        currentScore += amount;
        if (currentScore >= targetScore)
        {
            ChangeState(GameState.Win);
            return;
        }
        UpdateUI();
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
        // Handle state-specific logic here if needed
        Debug.Log("GameManager: Game State changed to: " + CurrentState);
        ShowPanel(CurrentState);
        UpdateUI();
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

    private void UpdateUI()
    {
        timerText.text = "Time: " + Mathf.CeilToInt(timeLeft).ToString();
        scoreText.text = "Score: " + currentScore.ToString() + "/" + targetScore.ToString();
    }
}
