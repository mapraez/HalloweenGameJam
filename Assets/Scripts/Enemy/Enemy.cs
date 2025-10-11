using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    [Header("Enemy Settings")]
    [SerializeField] private int health = 100;
    [SerializeField] private Bone bonePrefab;
    private int graveId;


    [Header("Enemy References")]

    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private Transform[] patrolPoints;
    private int currentPatrolIndex = 0;
    private NavMeshAgent agent;

    float attackCooldown = 1f;
    float lastAttackTime = -Mathf.Infinity;
    
    Coroutine attackCoroutine;

    public event Action<Enemy> OnEnemyDamaged;
    public event Action<Enemy> OnEnemyDeath;
    void Awake()
    {
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

    public void TakeDamage(int damage)
    {
        if (attackCoroutine != null) { return; }
        StopAllCoroutines();
        attackCoroutine = StartCoroutine(TakeDamageRoutine(damage));
    }

    private IEnumerator TakeDamageRoutine(int damage)
    {
        Debug.Log("Enemy took damage: " + damage);

        OnEnemyDamaged?.Invoke(this);
        health -= damage;
        SoundManager.Instance.PlaySoundEffect(hitSound);
        if (health <= 0)
        {
            Die();
        }
        yield return new WaitForSeconds(attackCooldown);
        Debug.Log("Enemy can take damage again");
        attackCoroutine = null;
    }


    private void Die()
    {
        Debug.Log("Enemy died");
        SoundManager.Instance.PlaySoundEffect(deathSound);
        DropBones();
        OnEnemyDeath?.Invoke(this);
        Destroy(gameObject);
    }



    private void DropBones()
    {
        if (bonePrefab != null)
        {
            Bone newBone = Instantiate(bonePrefab, transform.position, Quaternion.identity);
            newBone.SetBoneType(BoneType.FullSkeleton);
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
        TakeDamage(health);
    }
}
