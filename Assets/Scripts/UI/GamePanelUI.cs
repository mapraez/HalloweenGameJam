using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class GamePanelUI : MonoBehaviour
{
    [SerializeField] private Transform healthIconContainer;
    [SerializeField] private HealthBox healthIconPrefab;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetHealth(int health, int maxHealth)
    {
        ClearHealthIcons();


        for (int i = 0; i < health; i += 2)
        {
            HealthBox healthBox = Instantiate(healthIconPrefab, healthIconContainer);
            if (i + 1 < health)
            {
                healthBox.SetHealthState(2); // Full health
            }
            else if (i < health)
            {
                healthBox.SetHealthState(1); // Half health
            }
            else
            {
                healthBox.SetHealthState(0); // Empty health
            }
        }
    }

    private void ClearHealthIcons()
    {
        foreach (Transform child in healthIconContainer)
        {
            Destroy(child.gameObject);
        }
    }
    
}
