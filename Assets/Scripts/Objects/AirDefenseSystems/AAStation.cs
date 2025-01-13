using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AAStation : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private Transform missileSpawnPoint;
    [SerializeField] public float detectionRadius = 50f;
    [SerializeField] private float reloadTime = 2f;
    [SerializeField] private int maxMissiles = 10;

    private int currentMissiles;
    private Transform currentTarget;
    private bool isReloading;

    private MissileStorageManager missileStorageManager;
    private ConstructionProcess constructionProcess;

    private static readonly Dictionary<Transform, AAStation> lockedTargets = new Dictionary<Transform, AAStation>();

    private void Awake()
    {
        currentMissiles = maxMissiles;
        missileStorageManager = FindObjectOfType<MissileStorageManager>();
        constructionProcess = GetComponent<ConstructionProcess>();

        ValidateDependencies();
    }

    private void Update()
    {
        if (IsUnderConstruction()) return;
        HandleMissileRefill();
        HandleTargeting();
        HandleFiring();
    }

    private bool IsUnderConstruction() => constructionProcess != null && constructionProcess.ConstructionProgress < 1f;

    private void ValidateDependencies()
    {
        if (missileStorageManager == null)
            throw new MissingReferenceException("MissileStorageManager not found in the scene.");

        if (constructionProcess == null)
            Debug.LogWarning("ConstructionProcess component not found. Assuming station is pre-built.");
    }

    private void HandleMissileRefill()
    {
        if (currentMissiles >= maxMissiles) return;

        var storage = missileStorageManager?.GetNextStorage();
        if (storage == null) return;

        int refillAmount = Mathf.Min(storage.GetCurrentMissiles(), maxMissiles - currentMissiles);
        if (refillAmount > 0)
        {
            storage.RemoveMissiles(refillAmount);
            currentMissiles += refillAmount;
        }
    }

    private void HandleTargeting()
    {
        if (currentTarget != null && IsTargetInRange(currentTarget)) return;

        UnlockTarget();
        FindNewTarget();
    }

    private void FindNewTarget()
    {
        foreach (var hit in Physics.OverlapSphere(transform.position, detectionRadius))
        {
            if (!hit.CompareTag("Shahed") || lockedTargets.ContainsKey(hit.transform)) continue;

            LockTarget(hit.transform);
            break;
        }
    }

    private void LockTarget(Transform target)
    {
        lockedTargets[target] = this;
        currentTarget = target;
    }

    private void UnlockTarget()
    {
        if (currentTarget != null && lockedTargets.TryGetValue(currentTarget, out var station) && station == this)
        {
            lockedTargets.Remove(currentTarget);
        }

        currentTarget = null;
    }

    private bool IsTargetInRange(Transform target)
    {
        return target != null && Vector3.Distance(transform.position, target.position) <= detectionRadius;
    }

    private void HandleFiring()
    {
        if (currentTarget == null || isReloading || currentMissiles <= 0) return;

        FireMissile();
    }

    private void FireMissile()
    {
        currentMissiles--;
        CameraShake.Instance.ShakeWithDistance(gameObject.transform.position, 0.5f, 40f, 0.05f);
        var missile = Instantiate(missilePrefab, missileSpawnPoint.position, Quaternion.LookRotation(currentTarget.position - missileSpawnPoint.position));
        missile.GetComponent<Missile>().target = currentTarget;

        StartCoroutine(ReloadRoutine());
    }

    private IEnumerator ReloadRoutine()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
    }

    private void OnDestroy()
    {
        UnlockTarget();
    }

    public int GetCurrentMissiles() => currentMissiles;
    public int GetMaxMissiles() => maxMissiles;
    public float GetReloadTime() => reloadTime;
}
