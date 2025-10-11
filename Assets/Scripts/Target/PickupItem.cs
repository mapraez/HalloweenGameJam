using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private int scoreValue = 10;
    [SerializeField] private AudioClip hitSound;

    Collider targetCollider;

    void Awake()
    {
        targetCollider = GetComponent<Collider>();
    }

    void Start()
    {

    }

    public bool IsSpawnPositionValid()
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position, targetCollider.bounds.extents, Quaternion.identity);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider != targetCollider && hitCollider.GetComponent<PickupItem>() != null)
            {
                return false;
            }
        }
        return true;
    }


    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        GameManager.Instance.AddScore(scoreValue);
        SoundManager.Instance.PlaySoundEffect(hitSound);
        Destroy(gameObject);
    }

}