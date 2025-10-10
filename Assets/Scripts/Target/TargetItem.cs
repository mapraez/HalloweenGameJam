using UnityEngine;

public class TargetItem : MonoBehaviour
{
    [SerializeField] private int scoreValue = 10;
    [SerializeField] private AudioClip hitSound;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        GameManager.Instance.AddScore(scoreValue);
        SoundManager.Instance.PlaySoundEffect(hitSound);
        Destroy(gameObject);
    }
}
