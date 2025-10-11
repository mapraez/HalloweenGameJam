using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    [Header("Enemy Settings")]
    [SerializeField] private int health = 100;
    [SerializeField] private Bone bonePrefab;
    private Grave MyGrave { get; set; }



    [Header("Enemy References")]

    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private Transform[] patrolPoints;
    private int currentPatrolIndex = 0;
    private NavMeshAgent agent;


    public event Action<Enemy> OnEnemyDamaged;
    public event Action<Enemy> OnEnemyDeath;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
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
        if (agent.remainingDistance < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }

    }

    public void SetGrave(Grave grave)
    {
        MyGrave = grave;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        SoundManager.Instance.PlaySoundEffect(hitSound);
        if (health <= 0)
        {
            Die();
        }
    }


    private void Die()
    {
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
            newBone.SetGrave(MyGrave);
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
