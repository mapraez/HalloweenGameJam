using UnityEngine;
using UnityEngine.UI;

public class CharacterDisplay : MonoBehaviour
{

    [SerializeField] private Image HealthBarImage;

    Camera mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Always face the camera
        if (mainCamera != null)
    {
        // Copy camera's rotation exactly
        transform.rotation = mainCamera.transform.rotation;
    }
    }
    

    public void UpdateHealthBar(float healthPercent)
    {
        HealthBarImage.fillAmount = healthPercent;
    }
}
