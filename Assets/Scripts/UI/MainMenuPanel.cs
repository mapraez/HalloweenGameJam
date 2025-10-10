using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    [SerializeField] private Button ExitButton;

    void Awake()
    {
        ExitButton.onClick.AddListener(OnExitGameButtonClicked);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnExitGameButtonClicked()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
