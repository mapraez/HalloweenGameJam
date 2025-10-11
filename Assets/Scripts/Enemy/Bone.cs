using UnityEngine;

public class Bone : MonoBehaviour
{
    Grave associatedGrave;
    public BoneType boneType;

    public void SetBoneType(BoneType type)
    {
        boneType = type;
    }

    public void SetGrave(Grave grave)
    {
        associatedGrave = grave;
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
