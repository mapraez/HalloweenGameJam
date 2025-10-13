using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 720f;


    private Rigidbody rb;
    private Vector2 MovementInput => inputActions.Player.Move.ReadValue<Vector2>();
    private bool IsAttacking { get; set; }
    private LightWeapon lightWeapon;

    InputSystem_Actions inputActions;
    override protected void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        lightWeapon = GetComponentInChildren<LightWeapon>();
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();
        inputActions.Player.Attack.performed += ctx => Attack();
        inputActions.Player.Attack.canceled += ctx => StopAttack();
        inputActions.Player.Pause.performed += ctx => GameManager.Instance.TogglePause();
    }

    void Start()
    {
        SceneManager.sceneLoaded += HandleOnSceneLoaded;
        HandleOnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
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

        Debug.Log("Attack started");
        IsAttacking = true;
        lightWeapon.ToggleLights(true);
    }
    private void StopAttack()
    {
        if (!IsAttacking) return;

        Debug.Log("Attack stopped");
        IsAttacking = false;
        lightWeapon.ToggleLights(false);
    }

    private void HandleOnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Transform spawnPoint = StaticLocationManager.Instance.GetPlayerSpawnPoint();
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
        }
        else
        {
            Debug.LogWarning("PlayerController: No spawn point found for player.");
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }
    }


    void OnDestroy()
    {
        lightWeapon.ToggleLights(false);
        inputActions.Player.Attack.performed -= ctx => Attack();
        inputActions.Player.Attack.canceled -= ctx => StopAttack();
        inputActions.Player.Disable();
        inputActions.Dispose();
        SceneManager.sceneLoaded -= HandleOnSceneLoaded;
    }

}
