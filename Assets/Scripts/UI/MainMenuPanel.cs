using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    [SerializeField] private Button StartButton;
    [SerializeField] private Button ExitButton;

    void Awake()
    {
        StartButton.onClick.AddListener(OnStartGameButtonClicked);
        ExitButton.onClick.AddListener(OnExitGameButtonClicked);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    private void OnStartGameButtonClicked()
    {
        Debug.Log("Start Game button clicked");
        GameManager.Instance.StartGame();
    }
    
    private void OnExitGameButtonClicked()
    {
        Debug.Log("Exit Game button clicked");
        Application.Quit();
    }

}
