using System.Collections;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{

    [SerializeField] private TargetItem[] targetPrefabs;
    [SerializeField] private float spawnInterval = 2f;

    [SerializeField] private float spawnRadius = 5f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnRandomTargets());
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 randomPos = Random.insideUnitSphere * spawnRadius;
        randomPos.y = 0;
        return randomPos;
    }

    private IEnumerator SpawnRandomTargets()
    {
        while (true)
        {
            SpawnRandomTarget();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnRandomTarget()
    {
        int randomIndex = Random.Range(0, targetPrefabs.Length);
        SpawnTarget(randomIndex);
    }

    private void SpawnTarget(int index)
    {
        if (index < 0 || index >= targetPrefabs.Length) return;
        TargetItem target = Instantiate(targetPrefabs[index], GetRandomSpawnPosition(), Quaternion.identity);
    }

    [ContextMenu("Start Spawning Targets")]
    public void StartSpawning()
    {
        StartCoroutine(SpawnRandomTargets());
    }

    [ContextMenu("Stop Spawning Targets")]
    public void StopSpawning()
    {
        StopAllCoroutines();
    }
}
