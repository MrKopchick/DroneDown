using UnityEngine;

public class Shahed : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float diveSpeed = 15f;
    [SerializeField] private float diveDistance = 15f;
    [SerializeField] private float collisionThreshold = 1f;
    [SerializeField] private float wobbleIntensity = 0.5f;
    [SerializeField] private float wobbleFrequency = 2f;

    [SerializeField] private GameObject fireSmokeEffectPrefab; // Префаб ефекту вогню та диму

    public Vector3 targetPosition { get; set; }
    private HouseManager targetHouseManager;

    private bool isDiving = false;
    private Vector3 defaultDirection;

    public bool IsDiving => isDiving; 
    public float Speed => speed;
    public float DiveSpeed => diveSpeed;
    public Vector3 DefaultDirection => defaultDirection; 

    void Start()
    {
        ChooseNewDefaultDirection();
    }

    void Update()
    {
        if (targetPosition == Vector3.zero)
        {
            FlyStraight();
            return;
        }

        MoveTowardsTarget();

        if (Vector3.Distance(transform.position, targetPosition) <= collisionThreshold)
        {
            HandleCollisionWithTarget();
        }
    }

    private void MoveTowardsTarget()
    {
        Vector3 targetXZ = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        float distanceXZ = Vector3.Distance(transform.position, targetXZ);

        if (!isDiving && distanceXZ > diveDistance)
        {
            Vector3 direction = (targetXZ - transform.position).normalized;
            AddWobble(ref direction);
            transform.position += direction * speed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(direction);
        }
        else
        {
            isDiving = true;
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * diveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    public void SetTarget(Vector3 position, HouseManager houseManager)
    {
        targetPosition = position;
        targetHouseManager = houseManager;
    }

    private void FlyStraight()
    {
        Vector3 direction = defaultDirection;
        AddWobble(ref direction);
        transform.position += direction * speed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void ChooseNewDefaultDirection()
    {
        float randomAngle = Random.Range(0f, 360f);
        defaultDirection = new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle)).normalized;
    }

    private void AddWobble(ref Vector3 direction)
    {
        // Логіка для додання коливань
    }

    private void HandleCollisionWithTarget()
    {
        if (targetHouseManager != null && targetHouseManager.DamageHouse(targetPosition))
        {
            Debug.Log($"Shahed collided with house at {targetPosition}.");
        }
        else
        {
            Debug.Log($"Shahed reached target at {targetPosition}.");
        }

        // Спавн ефекту вогню та диму
        if (fireSmokeEffectPrefab != null)
        {
            Vector3 spawnPosition = targetPosition + Vector3.up; 
            Instantiate(fireSmokeEffectPrefab, spawnPosition, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
