using System.Collections;
using UnityEngine;

public class Hazard : MonoBehaviour
{

    [SerializeField] private int damageAmount = 1;
    [SerializeField] private Transform[] hazardPatrolPoints;
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float waitTimeAtPoint = 1f;

    [SerializeField] private bool flyUpAndDown = true;
    [SerializeField] private float flySpeed = 1f;
    [SerializeField] private float flyHeight = 0.5f;

    private bool inPlayerHitCooldown = false;

    private int currentPatrolIndex = 0;
    Transform currentTargetPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTargetPoint = hazardPatrolPoints[currentPatrolIndex];
        StartCoroutine(PatrolRoutine());

        if (flyUpAndDown) StartCoroutine(FlyRoutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator PatrolRoutine()
    {
        while (true)
        {
            if (hazardPatrolPoints.Length == 0) yield break;

            while (Vector3.Distance(transform.position, currentTargetPoint.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, currentTargetPoint.position, patrolSpeed * Time.deltaTime);
                yield return null;
            }
            currentPatrolIndex = (currentPatrolIndex + 1) % hazardPatrolPoints.Length;
            currentTargetPoint = hazardPatrolPoints[currentPatrolIndex];
            transform.LookAt(currentTargetPoint);
            yield return new WaitForSeconds(waitTimeAtPoint);
        }
    }

    private IEnumerator FlyRoutine()
    {
        float originalY = transform.position.y;
        while (true)
        {
            float newY = originalY + Mathf.Sin(Time.time * flySpeed) * flyHeight;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (inPlayerHitCooldown) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            inPlayerHitCooldown = true;
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                StartCoroutine(HitPlayerWithCooldown(player));

            }
        }
    }

    IEnumerator HitPlayerWithCooldown(PlayerController player)
    {
        Debug.Log("Hazard hit player for " + damageAmount + " damage.");
        player.TakeDamage(damageAmount);
        yield return new WaitForSeconds(1f); // Cooldown duration
        inPlayerHitCooldown = false;
    }

}
