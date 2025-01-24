using System;
using System.Collections.Generic;
using UnityEngine;

public class ShahedSpawnAI : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject shahedPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int minShahedPerAttack = 3;
    [SerializeField] private int maxShahedPerAttack = 10;
    [SerializeField] private float spawnInterval = 0.5f;

    [Header("Time Settings")]
    [SerializeField] private float baseAttackInterval = 600f;
    [SerializeField] private float nightAttackChance = 0.6f;
    [SerializeField] private float attackIntervalVariance = 300f;
    [SerializeField] private float minimumIntervalBetweenAttacks = 300f;

    [Header("Holiday Settings")]
    [SerializeField] private List<DateTime> holidays = new List<DateTime>();
    [SerializeField] private int holidayShahedMultiplier = 2;

    [Header("Target Settings")]
    [SerializeField] private float targetDetectionRadius = 50f;

    private float attackTimer;
    private float lastAttackTime;
    private int totalAttacks;
    private List<HouseManager> houseManagers = new List<HouseManager>();
    private List<Transform> activeTargets = new List<Transform>();

    public event Action OnAttack;

    private void Start()
    {
        attackTimer = GetNextAttackInterval();
        FindCities();
        InitializeHolidays();
    }

    private void Update()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0 && Time.time - lastAttackTime >= minimumIntervalBetweenAttacks)
        {
            StartCoroutine(ExecuteAttack());
            attackTimer = GetNextAttackInterval();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ExecuteAttack());
        }
    }

    private void FindCities()
    {
        houseManagers.Clear();
        HouseManager[] allHouseManagers = FindObjectsOfType<HouseManager>();
        foreach (var manager in allHouseManagers)
        {
            if (manager != null && !manager.IsEmpty())
            {
                houseManagers.Add(manager);
            }
        }

        if (houseManagers.Count == 0)
        {
            Debug.LogWarning("No cities with house managers found.");
        }
        else
        {
            Debug.Log($"Found {houseManagers.Count} cities with house managers.");
        }
    }


    private System.Collections.IEnumerator ExecuteAttack()
    {
        int shahedCount = UnityEngine.Random.Range(minShahedPerAttack, maxShahedPerAttack);
        if (IsHoliday(DateTime.Now))
        {
            shahedCount *= holidayShahedMultiplier;
        }

        for (int i = 0; i < shahedCount; i++)
        {
            Transform spawnPoint = GetRandomSpawnPoint();
            if (spawnPoint == null) continue;

            GameObject shahed = Instantiate(shahedPrefab, spawnPoint.position, spawnPoint.rotation);
            Shahed shahedScript = shahed.GetComponent<Shahed>();
            if (shahedScript != null)
            {
                AssignTarget(shahedScript);
            }

            yield return new WaitForSeconds(spawnInterval);
        }

        totalAttacks++;
        lastAttackTime = Time.time;
        OnAttack?.Invoke();
    }

    private void AssignTarget(Shahed shahedScript)
    {
        if (UnityEngine.Random.value < 0.5f)
        {
            Transform target = GetRandomTarget();
            if (target != null)
            {
                shahedScript.SetTarget(target.position, null);
            }
            else
            {
                AssignFallbackTarget(shahedScript);
            }
        }
        else
        {
            AssignFallbackTarget(shahedScript);
        }
    }

    private void AssignFallbackTarget(Shahed shahedScript)
    {
        if (houseManagers.Count == 0)
        {
            Debug.LogWarning("No house managers available for fallback targets.");
            return;
        }

        HouseManager selectedManager = houseManagers[UnityEngine.Random.Range(0, houseManagers.Count)];
        Vector3 housePosition = selectedManager.GetRandomHousePosition();

        if (housePosition != Vector3.zero)
        {
            shahedScript.SetTarget(housePosition, selectedManager);
        }
        else
        {
            Debug.LogWarning("No valid houses available in the selected city.");
        }
    }


    private Transform GetRandomTarget()
    {
        if (activeTargets.Count > 0)
        {
            return activeTargets[UnityEngine.Random.Range(0, activeTargets.Count)];
        }
        return null;
    }

    private Transform GetRandomSpawnPoint()
    {
        if (spawnPoints.Length == 0) return null;
        return spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
    }

    private float GetNextAttackInterval()
    {
        float interval = baseAttackInterval + UnityEngine.Random.Range(-attackIntervalVariance, attackIntervalVariance);
        if (UnityEngine.Random.value <= nightAttackChance && !IsNightTime())
        {
            interval += CalculateTimeToNextNight();
        }
        return Mathf.Max(60f, interval);
    }

    private bool IsNightTime()
    {
        int hour = DateTime.Now.Hour;
        return hour >= 22 || hour < 6;
    }

    private float CalculateTimeToNextNight()
    {
        int currentHour = DateTime.Now.Hour;
        if (currentHour >= 22) return (24 - currentHour) * 3600;
        return (22 - currentHour) * 3600;
    }

    private bool IsHoliday(DateTime date)
    {
        foreach (var holiday in holidays)
        {
            if (holiday.Date == date.Date) return true;
        }
        return false;
    }

    private void InitializeHolidays()
    {
        AddHoliday(new DateTime(DateTime.Now.Year, 1, 1));  // New Year
        AddHoliday(new DateTime(DateTime.Now.Year, 1, 7));  // Christmas
        AddHoliday(new DateTime(DateTime.Now.Year, 8, 24)); // Independence Day
        AddHoliday(new DateTime(DateTime.Now.Year, 12, 25));// Catholic Christmas
    }

    public void AddHoliday(DateTime holiday)
    {
        if (!holidays.Contains(holiday)) holidays.Add(holiday);
    }

    public void RegisterTarget(Transform target)
    {
        if (!activeTargets.Contains(target))
        {
            activeTargets.Add(target);
        }
    }

    public void UnregisterTarget(Transform target)
    {
        if (activeTargets.Contains(target))
        {
            activeTargets.Remove(target);
        }
    }

    public int GetTotalAttacks() => totalAttacks;
}
