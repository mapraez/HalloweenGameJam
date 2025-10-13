using UnityEngine;

public class StaticLocationManager : Singleton<StaticLocationManager>
{
    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform[] patrolPoints;

    [SerializeField] private Transform playerSpawnPoint;


    override protected void Awake()
    {
        base.Awake();
        Debug.Log("SpawnPointManager: Found " + spawnPoints.Length + " spawn points.");
    }
    public Vector3 GetRandomSpawnPoint()
    {

        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points available!");
            return Vector3.up; // Default fallback position
        }

        Transform selectedSpawnPoint = null;
        int attempts = 0;
        const int maxAttempts = 5;

        while (attempts < maxAttempts)
        {
            Transform candidate = spawnPoints[Random.Range(0, spawnPoints.Length)];
            if (!IsSpawnPointOccupied(candidate))
            {
                selectedSpawnPoint = candidate;
                break;
            }
            attempts++;
        }

        if (selectedSpawnPoint == null)
        {
            Debug.LogWarning("All spawn points are occupied. Selecting a random one anyway.");
            selectedSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        }

        Debug.Log("Selected spawn point at position: " + selectedSpawnPoint.position);
        return selectedSpawnPoint.position;
    }

    public bool IsSpawnPointOccupied(Transform spawnPoint)
    {
        Collider[] colliders = Physics.OverlapSphere(spawnPoint.position, 1f);

        bool isOccupied = false;
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                isOccupied = true;
                break;
            }
        }
        Debug.Log("Spawn point at " + spawnPoint.position + " occupied: " + isOccupied);
        return isOccupied;
    }

    public Transform[] GetPatrolPoints()
    {
        return patrolPoints;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawCube(transform.position, Vector3.one);

        if (spawnPoints != null)
        {
            Gizmos.color = Color.green;
            foreach (Transform point in spawnPoints)
            {
                if (point != null)
                {
                    Gizmos.DrawSphere(point.position, 0.3f);
                }
            }
        }

        if (patrolPoints != null)
        {
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                Transform point = patrolPoints[i];

                Gizmos.color = Color.cyan;
                if (point != null)
                {
                    Gizmos.DrawCube(point.position, Vector3.one * 0.3f);

                    Gizmos.color = Color.blue;
                    if (i < patrolPoints.Length - 1)
                    {
                        Gizmos.DrawLine(point.position, patrolPoints[i + 1].position);
                    }
                    else
                    {
                        Gizmos.DrawLine(point.position, patrolPoints[0].position);
                    }
                }
            }
        }
    }

    public Transform GetPlayerSpawnPoint()
    {
        return playerSpawnPoint;
    }
}
