using System;
using System.Collections;
using Unity.Mathematics;
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

    [SerializeField] GameObject clearedEnemyObject;
    [SerializeField] GameObject closedGraveObject;

    [SerializeField] GameObject openGraveVFXPrefab;
    [SerializeField] GameObject closeGraveVFXPrefab;

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
        Invoke("SpawnEnemy", 2f); // Delay spawn for demonstration
    }


    [ContextMenu("Spawn Enemy")]
    public void SpawnEnemy()
    {
        if (mySkeleton != null) return;
        mySkeleton = Instantiate(enemyPrefab, StaticLocationManager.Instance.GetRandomSpawnPoint(), Quaternion.identity);
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
            StartCoroutine(CloseGraveRoutine());
        }

    }

    IEnumerator CloseGraveRoutine()
    {
        Debug.Log("Grave: Closing grave " + graveId);

        yield return new WaitForSeconds(0.5f);
        clearedEnemyObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        closedGraveObject.SetActive(true);

        if (closeGraveVFXPrefab != null)
        {
            GameObject vfx = Instantiate(closeGraveVFXPrefab, transform.position + Vector3.up * .5f + Vector3.back * 1.5f, Quaternion.identity);
            Destroy(vfx, 3f);
        }
        while (closedGraveObject.transform.localPosition.x > 0f)
        {
            closedGraveObject.transform.localPosition += Vector3.left * Time.deltaTime;
            yield return null;
        }
        while (closedGraveObject.transform.localPosition.y > .6f)
        {
            closedGraveObject.transform.localPosition += Vector3.down * Time.deltaTime;
            yield return null;
        }
        OnGraveCleared?.Invoke(this);

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
