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
        if (mainCamera != null)
        {
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward);
        }
    }
    

    public void UpdateHealthBar(float healthPercent)
    {
        HealthBarImage.fillAmount = healthPercent;
    }
}
