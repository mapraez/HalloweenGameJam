using TMPro;
using UnityEngine;

public class ScoreObject : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI nameText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetName(GameManager.Instance.PlayerName);
        SetScore(GameManager.Instance.CurrentScore);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }
    
    public void SetName(string name)
    {
        if (nameText != null)
        {
            nameText.text = name;
        }
    }
}
