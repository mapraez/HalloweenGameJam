using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Panels")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject levelMenuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject levelCompletePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("Sound Settings")]
    [SerializeField] private Slider backgroundMusicVolumeSlider;
    [SerializeField] private Slider soundEffectVolumeSlider;

    override protected void Awake()
    {
        base.Awake();

    }

    void Start()
    {
        backgroundMusicVolumeSlider.value = SoundManager.Instance.GetBackgroundMusicVolume();
        soundEffectVolumeSlider.value = SoundManager.Instance.GetSoundEffectVolume();
        backgroundMusicVolumeSlider.onValueChanged.AddListener(SoundManager.Instance.SetBackgroundMusicVolume);
        soundEffectVolumeSlider.onValueChanged.AddListener(SoundManager.Instance.SetSoundEffectVolume);
    }

    public void ShowPanel(GameState state)
    {
        settingsPanel.SetActive(state == GameState.MainMenu || state == GameState.Paused);

        menuPanel.SetActive(state == GameState.MainMenu);
        levelMenuPanel.SetActive(state == GameState.LevelMenu);
        gamePanel.SetActive(state == GameState.Playing);
        pausePanel.SetActive(state == GameState.Paused);
        levelCompletePanel.SetActive(state == GameState.LevelComplete);
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

    public void CallStartGame()
    {
        GameManager.Instance.StartGame();
    }

    public void CallStartLevel()
    {
        GameManager.Instance.StartCurrentLevel();
    }

    public void CallNextLevel()
    {
        GameManager.Instance.LoadNextLevel();
    }

    public void CallQuitToMenu()
    {
        GameManager.Instance.GoToMainMenu();
    }


    public void CallExitGame()
    {
        GameManager.Instance.ExitGame();
    }

    public void UpdateUI(GameState currentGameState, float timeLeft, int currentScore, int targetScore)
    {
        ShowPanel(currentGameState);
        UpdateTimer(timeLeft);
        UpdateScore(currentScore, targetScore);
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        gamePanel.GetComponent<GamePanelUI>().SetHealth(currentHealth, maxHealth);
    }

    void OnDestroy()
    {
        backgroundMusicVolumeSlider.onValueChanged.RemoveListener(SoundManager.Instance.SetBackgroundMusicVolume);
        soundEffectVolumeSlider.onValueChanged.RemoveListener(SoundManager.Instance.SetSoundEffectVolume);
    }
}
