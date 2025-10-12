using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject gameOverPanel;

    override protected void Awake()
    {
        base.Awake();
    }

    public void ShowPanel(GameState state)
    {
        menuPanel.SetActive(state == GameState.MainMenu);
        gamePanel.SetActive(state == GameState.Playing);
        pausePanel.SetActive(state == GameState.Paused);
        winPanel.SetActive(state == GameState.Win);
        losePanel.SetActive(state == GameState.Lose);
        gameOverPanel.SetActive(state == GameState.GameOver);
    }


    public void UpdateScore(int currentScore, int targetScore)
    {
        scoreText.text = "Score: " + currentScore.ToString() + "/" + targetScore.ToString();
    }

    public void UpdateTimer(float timeLeft)
    {
        timerText.text = "Time: " + Mathf.CeilToInt(timeLeft).ToString();
    }

}
