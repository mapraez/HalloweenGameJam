using UnityEngine;

public class EndGameTV : MonoBehaviour
{

    [SerializeField] private GameObject staticScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameManager.Instance.CurrentGameState == GameState.Lose)
        {
            staticScreen.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
