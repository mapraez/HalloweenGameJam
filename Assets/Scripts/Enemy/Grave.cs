using System;
using UnityEngine;

public class Grave : MonoBehaviour
{
    private static int nextGraveId = 1; // Static counter for unique IDs
    
    [SerializeField] private int graveId; // Made private and serialized for inspector visibility
    public int GraveId => graveId; // Public property to access the ID

    [SerializeField] private Enemy enemyPrefab;
    private Enemy mySkeleton;
    public static event Action<Grave> OnGraveSpawned;
    public static event Action<Grave> OnGraveCleared;

    void Awake()
    {
        // Assign unique ID if not already set
        if (graveId == 0)
        {
            graveId = nextGraveId++;
        }
        else
        {
            // Update the counter to prevent conflicts with manually set IDs
            nextGraveId = Mathf.Max(nextGraveId, graveId + 1);
        }
    }

    void Start()
    {
        OnGraveSpawned?.Invoke(this);
    }


    [ContextMenu("Spawn Enemy")]
    public void SpawnEnemy()
    {
        if (mySkeleton != null) return;
        mySkeleton = Instantiate(enemyPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        mySkeleton.SetGrave(graveId);
        mySkeleton.OnEnemyCollected += HandleEnemyCollected;
    }

    private void HandleEnemyCollected(Enemy enemy)
    {
        if (enemy != mySkeleton) return;
        ClearGrave();
    }


    [ContextMenu("Clear Grave")]
    public void ClearGrave()
    {
        if (mySkeleton != null)
        {
            Debug.Log("Grave: Enemy collected for grave " + graveId);
            mySkeleton.OnEnemyCollected -= HandleEnemyCollected;
            Destroy(mySkeleton.gameObject);
            mySkeleton = null;
            OnGraveCleared?.Invoke(this);
        }
    }

    // Utility methods for managing grave IDs
    public static int GetNextGraveId()
    {
        return nextGraveId++;
    }

    public static void ResetGraveIdCounter()
    {
        nextGraveId = 1;
    }

    // Context menu for testing
    [ContextMenu("Assign New Unique ID")]
    private void AssignNewUniqueId()
    {
        graveId = GetNextGraveId();
        Debug.Log($"Assigned new grave ID: {graveId}");
    }

}
