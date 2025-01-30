using UnityEngine;

public abstract class AAStationBase : MonoBehaviour
{
    [Header("Common Settings")]
    [SerializeField] private float detectionRadius;
    [SerializeField] private float reloadTime;
    [SerializeField] private int maxAmmo;

    private int currentAmmo;
    private bool isReloading;
    protected Transform currentTarget;
    public float GetDetectionRadius() => detectionRadius;
    public int GetCurrentAmmo() => currentAmmo;
    public int GetMaxAmmo() => maxAmmo;
    public float GetReloadTime() => reloadTime;
    protected Transform CurrentTarget => currentTarget;

    protected virtual void Start()
    {
        currentAmmo = maxAmmo;
    }

    protected virtual void Update()
    {
        if (currentTarget == null || !IsTargetInRange(currentTarget))
        {
            FindNewTarget();
        }

        if (currentTarget != null && CanFire())
        {
            Vector3 predictedPosition = PredictTargetPosition();
            FireWeapon(predictedPosition);
        }

        if (currentAmmo <= 0 && !isReloading)
        {
            StartReloading();
        }
    }

    protected abstract void FireWeapon(Vector3 predictedPosition);

    protected virtual void FindNewTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Shahed"))
            {
                currentTarget = hit.transform;
                break;
            }
        }
    }

    protected virtual bool CanFire()
    {
        return currentAmmo > 0 && !isReloading;
    }

    private void StartReloading()
    {
        isReloading = true;
        Invoke(nameof(FinishReloading), reloadTime);
    }

    private void FinishReloading()
    {
        currentAmmo = maxAmmo;
        isReloading = false;
    }

    protected virtual bool IsTargetInRange(Transform target)
    {
        if (target == null) return false;
        return Vector3.Distance(transform.position, target.position) <= detectionRadius;
    }

    protected void DecreaseAmmo()
    {
        if (currentAmmo > 0)
        {
            currentAmmo--;
        }
    }

    protected virtual Vector3 PredictTargetPosition()
    {
        Rigidbody targetRb = currentTarget?.GetComponent<Rigidbody>();
        if (targetRb == null) return currentTarget.position;

        Vector3 targetVelocity = targetRb.velocity;
        Vector3 directionToTarget = currentTarget.position - transform.position;
        float distanceToTarget = directionToTarget.magnitude;

        float flightTime = distanceToTarget / 20f;
        return currentTarget.position + targetVelocity * flightTime;
    }
}
