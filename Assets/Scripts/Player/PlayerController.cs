using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private int maxHealth = 6;
    public int CurrentHealth { get; private set; }
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
        if (GameManager.Instance.CurrentGameState != GameState.Playing) return;
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
        ResetPlayer();

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

    [ContextMenu("Reset Player")]
    public void ResetPlayer()
    {
        CurrentHealth = maxHealth;
        UIManager.Instance.UpdateHealth(CurrentHealth, maxHealth);
        Debug.Log("Player health reset to max: " + maxHealth);
    }

    [ContextMenu("Test Take Damage")]
    private void TestTakeDamage()
    {
        TakeDamage(1);
    }

    [ContextMenu("Test Kill Player")]
    private void TestKillPlayer()
    {
        TakeDamage(CurrentHealth);
    }

    public void TakeDamage(int damage)
    {
        if (GameManager.Instance.CurrentGameState != GameState.Playing) return;

        CurrentHealth -= damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);
        Debug.Log("Player took damage. Current health: " + CurrentHealth);

        UIManager.Instance.UpdateHealth(CurrentHealth, maxHealth);

        if (CurrentHealth <= 0)
        {
            GameManager.Instance.ChangeState(GameState.Lose);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Player collided with enemy: " + collision.gameObject.name);
            TakeDamage(1);
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
