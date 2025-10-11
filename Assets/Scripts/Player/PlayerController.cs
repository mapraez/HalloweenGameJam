using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 720f;

    private Rigidbody rb;
    private Vector2 MovementInput => inputActions.Player.Move.ReadValue<Vector2>();
    private bool IsAttacking { get; set; }

    InputSystem_Actions inputActions;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
            Debug.Log("Attacking...");
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
        // Implement attack logic here
        Debug.Log("Attack started");
        IsAttacking = true;
    }
    private void StopAttack()
    {
        // Implement stop attack logic here
        Debug.Log("Attack stopped");
        IsAttacking = false;
    }

}
