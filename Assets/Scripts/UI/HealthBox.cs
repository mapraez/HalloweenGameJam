using UnityEngine;

public class HealthBox : MonoBehaviour
{

    [SerializeField] private GameObject fullHealthIcon;
    [SerializeField] private GameObject halfHealthIcon;
    [SerializeField] private GameObject emptyHealthIcon;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void SetHealthState(int health)
    {
        switch (health)
        {
            case 2:
                if (fullHealthIcon != null) fullHealthIcon.SetActive(true);
                if (halfHealthIcon != null) halfHealthIcon.SetActive(false);
                if (emptyHealthIcon != null) emptyHealthIcon.SetActive(false);
                break;
            case 1:
                if (fullHealthIcon != null) fullHealthIcon.SetActive(false);
                if (halfHealthIcon != null) halfHealthIcon.SetActive(true);
                if (emptyHealthIcon != null) emptyHealthIcon.SetActive(false);
                break;
            case 0:
                if (fullHealthIcon != null) fullHealthIcon.SetActive(false);
                if (halfHealthIcon != null) halfHealthIcon.SetActive(false);
                if (emptyHealthIcon != null) emptyHealthIcon.SetActive(true);
                break;
            default:
                Debug.LogWarning("Invalid health value: " + health);
                break;
        }
    }
}
