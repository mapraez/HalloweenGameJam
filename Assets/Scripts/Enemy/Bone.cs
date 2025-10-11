using UnityEngine;

public class Bone : MonoBehaviour
{
    [SerializeField] private int scoreValue = 5;
    [SerializeField] private AudioClip pickupSound;
    public BoneType boneType;
    private int graveId;

    public void SetBoneType(BoneType type)
    {
        boneType = type;
    }

    public void SetGraveId(int graveId)
    {
        this.graveId = graveId;
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        other.GetComponent<PlayerController>().CollectBone(boneType, graveId);
        GameManager.Instance.AddScore(scoreValue);
        SoundManager.Instance.PlaySoundEffect(pickupSound);
        Destroy(gameObject);
    }
}

public enum BoneType
{
    None,
    Bone1,
    Bone2,
    CrossBones,
    Skull,
    Ribcage,
    FullSkeleton
}
