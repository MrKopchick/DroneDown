using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;


public class Shahed : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float diveSpeed = 15f;
    [SerializeField] private float diveDistance = 15f;
    [SerializeField] private float collisionThreshold = 1f;
    [SerializeField] private float wobbleIntensity = 0.5f;
    [SerializeField] private float wobbleFrequency = 2f;

    [SerializeField] private GameObject fireSmokeEffectPrefab;

    private Vector3 targetPosition;
    private HouseManager targetHouseManager;

    private bool isDiving;
    private Vector3 defaultDirection;

    public bool IsDiving => isDiving;
    public float Speed => speed;
    public float DiveSpeed => diveSpeed;
    public Vector3 DefaultDirection => defaultDirection;

    public void SetTarget(Vector3 position, HouseManager houseManager)
    {
        targetPosition = position;
        targetHouseManager = houseManager;
    }

    private void Start()
    {
        ChooseNewDefaultDirection();
    }

    private void Update()
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
            ApplyWobble(ref direction);
            transform.position += direction * speed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(direction);
        }
        else
        {
            isDiving = true;
            Vector3 direction = (targetPosition - transform.position).normalized;
            ApplyDive(ref direction);
            transform.position += direction * diveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void ApplyDive(ref Vector3 direction)
    {
        float yDifference = targetPosition.y - transform.position.y;
        direction.y = yDifference > 0 ? 1 : (yDifference < 0 ? -1 : 0);
    }

    private void ApplyWobble(ref Vector3 direction)
    {
        float wobble = Mathf.Sin(Time.time * wobbleFrequency) * wobbleIntensity;
        direction.x += wobble;
        direction.z += wobble;
    }

    private void FlyStraight()
    {
        Vector3 direction = defaultDirection;
        ApplyWobble(ref direction);
        transform.position += direction * speed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void ChooseNewDefaultDirection()
    {
        float randomAngle = Random.Range(0f, 360f);
        defaultDirection = new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle)).normalized;
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

        if (fireSmokeEffectPrefab != null)
        {
            Instantiate(fireSmokeEffectPrefab, targetPosition + Vector3.up, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
