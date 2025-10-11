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

    // Update is called once per frame
    void Update()
    {

    }
    
    
    private void OnExitGameButtonClicked()
    {
        Debug.Log("Exit Game button clicked");
        Application.Quit();
    }

}
