using TMPro;
using UnityEngine;

public class ScoreObject : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI scoreText;

    void OnEnable()
    {
        SetName(GameManager.Instance.PlayerName);
        SetScore(GameManager.Instance.FinalScore);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetName(string name)
    {
        if (nameText != null)
        {
            nameText.text = name;
        }
    }
    
    public void SetScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Final Score: " + score.ToString();
        }
    }
    

}
