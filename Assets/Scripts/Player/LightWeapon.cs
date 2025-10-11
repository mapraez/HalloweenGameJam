using System.Collections.Generic;
using UnityEngine;

public class LightWeapon : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float range = 5f;
    [SerializeField] private Light[] lights;

    private bool isAttacking = false;


    void Awake()
    {
        lights = GetComponentsInChildren<Light>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ToggleLights(false);

    }

    // Update is called once per frame
    void Update()
    {
        HandleLightAttack();

    }

    public void ToggleLights(bool state)
    {
        isAttacking = state;
        foreach (Light light in lights)
        {
            light.enabled = state;
        }
    }

    private void HandleLightAttack()
    {
        if (!isAttacking) return;

        List<Enemy> hitEnemies = new List<Enemy>();

        foreach (Light light in lights)
        {
            RaycastHit[] hits = Physics.RaycastAll(light.transform.position, light.transform.forward, range, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            foreach (RaycastHit hit in hits)
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null && !hitEnemies.Contains(enemy))
                {
                    // Debug.Log("Light: " + light.name + " Raycast hitting enemy: " + enemy.name);
                    hitEnemies.Add(enemy);
                }
                
            }
        }

        foreach (Enemy enemy in hitEnemies)
        {
            enemy.TakeDamage(damage);
        }


        
    }

    void OnDrawGizmosSelected()
    {
        foreach (Light light in lights)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(light.transform.position, light.transform.forward * range);
        }
        
    }
}
