using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 720f;


    private Dictionary<int, List<BoneType>> collectedBonesByGrave = new Dictionary<int, List<BoneType>>();

    private Rigidbody rb;
    private Vector2 MovementInput => inputActions.Player.Move.ReadValue<Vector2>();
    private bool IsAttacking { get; set; }
    private LightWeapon lightWeapon;

    InputSystem_Actions inputActions;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        lightWeapon = GetComponentInChildren<LightWeapon>();
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();
        inputActions.Player.Attack.performed += ctx => Attack();
        inputActions.Player.Attack.canceled += ctx => StopAttack();
    }

    private void Update()
    {
        // Handle any per-frame updates here if necessary
        if (IsAttacking)
        {
            // Implement attack logic here, e.g., check for enemies in range
            // Debug.Log("Attacking...");
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + new Vector3(MovementInput.x, 0, MovementInput.y) * moveSpeed * Time.fixedDeltaTime);
        if (MovementInput != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(new Vector3(MovementInput.x, 0, MovementInput.y), Vector3.up);
            rb.rotation = Quaternion.RotateTowards(rb.rotation, toRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
    
    public void CollectBone(PickupItem targetBone)
    {
        Bone collectedBone = targetBone.GetComponent<Bone>();
        if (collectedBone == null) return;

    }
    private void Attack()
    {
        if (GameManager.Instance.CurrentState != GameState.Playing) return;
        if (IsAttacking) return;
        // Implement attack logic here
        Debug.Log("Attack started");
        IsAttacking = true;
        lightWeapon.ToggleLights(true);
    }
    private void StopAttack()
    {
        if (!IsAttacking) return;
        // Implement stop attack logic here
        Debug.Log("Attack stopped");
        IsAttacking = false;
        lightWeapon.ToggleLights(false);
    }

    public void CollectBone(BoneType boneType, int graveId)
    {
        Debug.Log($"Collected bone of type: {boneType} from grave ID: {graveId}");
        // Implement additional logic for collecting the bone, e.g., updating inventory
        // Store the collected bone associated with the grave
        if (!collectedBonesByGrave.ContainsKey(graveId))
        {
            collectedBonesByGrave[graveId] = new List<BoneType>();
        }
        collectedBonesByGrave[graveId].Add(boneType);
    }

    [ContextMenu("Print Collected Bones")]
    private void PrintCollectedBones()
    {
        foreach (var entry in collectedBonesByGrave)
        {
            int graveId = entry.Key;
            List<BoneType> bones = entry.Value;
            Debug.Log($"Grave ID: {graveId}, Collected Bones: {string.Join(", ", bones)}");
        }
    }
}
