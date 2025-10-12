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


    [Header("Enemy References")]

    [SerializeField] private AudioClip[] hitSounds;
    [SerializeField] private AudioClip deathSound;
    private int currentPatrolIndex = 0;

    [Header("UI References")]
    [SerializeField] private CharacterDisplay characterDisplay;

    private NavMeshAgent agent;
    private Transform[] patrolPoints = new Transform[0];

    float hitCooldown = 1f;
    Coroutine hitCoroutine;

    public event Action<Enemy> OnEnemyCollected;


    void Awake()
    {
        characterDisplay = GetComponentInChildren<CharacterDisplay>();
        agent = GetComponent<NavMeshAgent>();
        if (patrolPoints.Length == 0)
        {
            Debug.LogWarning("No patrol points assigned to enemy: " + name);
            patrolPoints = new Transform[0];
            
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
        currentHealth = maxHealth;
        characterDisplay.UpdateHealthBar(1f);
    }

    // Update is called once per frame
    void Update()
    {

        if (patrolPoints.Length == 0) return;
        if (agent.remainingDistance < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }

    }

    public void SetGrave(int graveId)
    {
        this.graveId = graveId;
    }

    public void SetPatrolPoints(Transform[] points)
    {
        patrolPoints = points;
        if (patrolPoints.Length > 0 && agent != null)
        {
            currentPatrolIndex = 0;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    public void TakeDamage(int damage)
    {
        if (hitCoroutine != null) { return; }
        StopAllCoroutines();
        hitCoroutine = StartCoroutine(TakeDamageRoutine(damage));
    }

    private IEnumerator TakeDamageRoutine(int damage)
    {
        Debug.Log("Enemy took damage: " + damage);

        // Damage Animation could go here
        currentHealth -= damage;
        SoundManager.Instance.PlaySoundEffect(hitSounds[Random.Range(0, hitSounds.Length)]);
        characterDisplay.UpdateHealthBar((float)currentHealth / maxHealth);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        yield return new WaitForSeconds(hitCooldown);
        Debug.Log("Enemy can take damage again");
        hitCoroutine = null;
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
