using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{

    [Header("Enemy Settings")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    [SerializeField] private Bone bonePrefab;
    private int graveId;

    [SerializeField] float patrolSpeed = 2f;
    [SerializeField] float patrolWaitTime = 2f;

    [Header("Enemy References")]

    [SerializeField] private AudioClip[] hitSounds;
    [SerializeField] private AudioClip deathSound;
    private int currentPatrolIndex;
    private int nextPatrolIndex;

    [Header("UI References")]
    [SerializeField] private CharacterDisplay characterDisplay;

    private NavMeshAgent agent;
    private Transform[] patrolPoints;


    float hitCooldown = 1f;


    Coroutine currentDamageRouting;
    Coroutine currentPatrolRoutine;

    public event Action<Enemy> OnEnemyCollected;


    void Awake()
    {
        characterDisplay = GetComponentInChildren<CharacterDisplay>();
        agent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
        
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterDisplay.UpdateHealthBar(1f);
        SetPatrolPoints();
        currentPatrolRoutine = StartCoroutine(StartPatrollingRoutine());
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void SetGrave(int graveId)
    {
        this.graveId = graveId;
    }

    public IEnumerator StartPatrollingRoutine()
    {
        agent.speed = patrolSpeed;
        yield return new WaitForSeconds(patrolWaitTime);

        if (patrolPoints.Length > 0 && agent != null)
        {
            while (true)
            {
                agent.SetDestination(patrolPoints[currentPatrolIndex].position);

                nextPatrolIndex = currentPatrolIndex + 1;
                if (nextPatrolIndex >= patrolPoints.Length)
                {
                    nextPatrolIndex = 0;
                }
                currentPatrolIndex = nextPatrolIndex;
                
                yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);
                yield return new WaitForSeconds(patrolWaitTime);
            }
        }
    }

    private void SetPatrolPoints()
    {
        Transform[] points = StaticLocationManager.Instance.GetPatrolPoints();
        patrolPoints = points;

        Debug.Log("Enemy: CurrentLocation: " + transform.position);

        float closestDistance = Mathf.Infinity;
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, patrolPoints[i].position);
            Debug.Log("Distance to patrol point " + i + ": " + distance);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                currentPatrolIndex = i;
            }
        }
        Debug.Log("Enemy: Closest patrol point index: " + currentPatrolIndex);
        
    }

    public void TakeDamage(int damage)
    {
        if (currentDamageRouting != null)
        {
            // Debug.Log("Enemy is already taking damage, ignoring new damage");
            return;
        }
        
        StopCoroutine(currentPatrolRoutine);
        currentDamageRouting = StartCoroutine(TakeDamageRoutine(damage));
    }

    private IEnumerator TakeDamageRoutine(int damage)
    {
        Debug.Log("Enemy took damage: " + damage);

        currentHealth -= damage;
        SoundManager.Instance.PlaySoundEffect(hitSounds[Random.Range(0, hitSounds.Length)]);

        // Damage Animation could go here

        characterDisplay.UpdateHealthBar((float)currentHealth / maxHealth);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        currentPatrolRoutine = StartCoroutine(StartPatrollingRoutine());

        yield return new WaitForSeconds(hitCooldown);
        currentDamageRouting = null;

        Debug.Log("Enemy can take damage again");
    }


    private void Die()
    {
        Debug.Log("Enemy died");
        SoundManager.Instance.PlaySoundEffect(deathSound);
        DropBones();
        OnEnemyCollected?.Invoke(this);
        Destroy(gameObject);
    }



    private void DropBones()
    {
        if (bonePrefab != null)
        {
            Bone newBone = Instantiate(bonePrefab, transform.position, Quaternion.identity);
            newBone.SetGraveId(graveId);
        }

    }


    [ContextMenu("Inflict Damage")]
    private void InflictDamage()
    {
        TakeDamage(10);
    }

    [ContextMenu("Kill Enemy")]
    private void KillEnemy()
    {
        TakeDamage(maxHealth);
    }
}
