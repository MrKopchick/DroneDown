using UnityEngine;
using System;

public class Missile : MonoBehaviour
{
    [Header("Missile Settings")]
    [SerializeField] private float initialSpeed = 10f;
    [SerializeField] private float maxSpeed = 40f;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float rotationSpeed = 60f;
    [SerializeField] private float collisionRadius = 1f;
    [SerializeField] private float maxLifetime = 10f;

    [Header("Visual Effects")]
    [SerializeField] private ParticleSystem trailEffect;
    [SerializeField] private Light missileLight;
    [SerializeField] private Transform modelTransform;

    private float currentSpeed;
    private bool isExploded;
    public Transform target;
    private GameObject trailInstance;

    public event Action<Missile> OnMissed;
    public event Action<Missile> OnDestroyed;

    #region Initialization

    private void Start()
    {
        if (trailEffect != null)
        {
            trailInstance = Instantiate(trailEffect.gameObject, transform.position, transform.rotation);
            trailInstance.transform.parent = null;
            trailEffect = trailInstance.GetComponent<ParticleSystem>();
            trailEffect.Play();
        }

        if (missileLight != null)
        {
            missileLight.enabled = true;
        }

        currentSpeed = initialSpeed;
        Invoke(nameof(StopEffects), 5f);
        Invoke(nameof(DestroyMissile), maxLifetime);
    }
    #endregion

    #region Unity Callbacks

    private void Update()
    {
        if (isExploded || target == null)
        {
            HandleMiss();
            return;
        }

        MoveTowardsTarget();
        UpdateTrailPosition();

        if (IsTargetHit())
        {
            Explode();
        }
    }

    private void OnDestroy()
    {
        CleanupTrail();
    }

    #endregion

    #region Core Logic

    private void MoveTowardsTarget()
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.position += transform.forward * currentSpeed * Time.deltaTime;

        currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);
    }
    private bool IsTargetHit()
    {
        return Vector3.Distance(transform.position, target.position) <= collisionRadius;
    }
    private void Explode()
    {
        if (isExploded) return;

        isExploded = true;

        if (target != null)
        {
            Destroy(target.gameObject);
        }

        StopEffects();
        DestroyMissile();
    }

    private void HandleMiss()
    {
        if (isExploded) return;

        StopEffects();
        OnMissed?.Invoke(this);
        DestroyMissile();
    }

    private void StopEffects()
    {
        if (trailEffect != null)
        {
            trailEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        if (missileLight != null)
        {
            missileLight.enabled = false;
        }
    }

    #endregion

    #region Trail Handling

    private void UpdateTrailPosition()
    {
        if (trailInstance != null)
        {
            trailInstance.transform.position = transform.position;
        }
    }

    private void CleanupTrail()
    {
        if (trailInstance != null)
        {
            Destroy(trailInstance, 5f);
        }
    }

    #endregion

    #region Destruction

    private void DestroyMissile()
    {
        OnDestroyed?.Invoke(this);
        CleanupTrail();
        Destroy(gameObject);
    }

    #endregion
}
