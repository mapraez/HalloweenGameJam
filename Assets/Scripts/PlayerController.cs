using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 720f;

    private Rigidbody rb;
    private Vector2 MovementInput => inputActions.Player.Move.ReadValue<Vector2>();

    InputSystem_Actions inputActions;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();
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
    

}
