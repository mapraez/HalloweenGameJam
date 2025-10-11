using UnityEngine;

public class Grave : MonoBehaviour
{

    public int graveId;

    [SerializeField] private Enemy enemyPrefab;
    private Enemy mySkeleton;

    void Start()
    {
        
    }


    [ContextMenu("Spawn Enemy")]
    public void SpawnEnemy()
    {
        if (mySkeleton != null) return;
        mySkeleton = Instantiate(enemyPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        mySkeleton.SetGrave(this);
    }

}
