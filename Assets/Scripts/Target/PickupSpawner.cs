using System.Collections;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{

    [SerializeField] private PickupItem[] itemPrefabs;
    [SerializeField] private float spawnInterval = 2f;


    [SerializeField] private Transform[] spawnPoints;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnRandomItems());
    }

    private Vector3 GetRandomSpawnPosition()
    {
        if (spawnPoints.Length == 0) return transform.position;
        int randomIndex = Random.Range(0, spawnPoints.Length);
        return spawnPoints[randomIndex].position;
    }

    private IEnumerator SpawnRandomItems()
    {
        yield return new WaitForSeconds(spawnInterval);
        while (true)
        {

            SpawnRandomItem();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnRandomItem()
    {
        int randomIndex = Random.Range(0, itemPrefabs.Length);
        SpawnItem(randomIndex);
    }

    private void SpawnItem(int index)
    {
        if (index < 0 || index >= itemPrefabs.Length) return;
        PickupItem target = Instantiate(itemPrefabs[index], GetRandomSpawnPosition(), Quaternion.identity);
        if (!target.IsSpawnPositionValid())
        {
            Destroy(target.gameObject);
        }
    }

    [ContextMenu("Start Spawning Items")]
    public void StartSpawning()
    {
        StartCoroutine(SpawnRandomItems());
    }

    [ContextMenu("Stop Spawning Items")]
    public void StopSpawning()
    {
        StopAllCoroutines();
    }
}
