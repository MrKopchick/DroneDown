using System.Collections.Generic;
using UnityEngine;

public class ShahedSpawner : MonoBehaviour
{
    [Header("Shahed Settings")]
    [SerializeField] private GameObject shahedPrefab;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private Transform[] spawnPoints;

    private float timer;
    private List<HouseManager> houseManagers = new List<HouseManager>();
    private bool isSpawningEnabled = false;

    private void Start()
    {
        FindCities();
        EnableSpawning();
    }

    private void Update()
    {
        if (!isSpawningEnabled) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnShahed();
            timer = 0;
        }
    }

    private void FindCities()
    {
        houseManagers.Clear();

        HouseManager[] allHouseManagers = FindObjectsOfType<HouseManager>();
        foreach (var manager in allHouseManagers)
        {
            if (manager != null && !houseManagers.Contains(manager))
            {
                houseManagers.Add(manager);
            }
        }

        Debug.Log($"Found {houseManagers.Count} cities with house managers.");
    }

    private void SpawnShahed()
    {
        if (houseManagers.Count == 0)
        {
            Debug.LogWarning("No cities available for targeting.");
            return;
        }

        Transform spawnPoint = GetRandomSpawnPoint();
        if (spawnPoint == null) return;

        GameObject shahed = Instantiate(shahedPrefab, spawnPoint.position, spawnPoint.rotation);
        Shahed shahedScript = shahed.GetComponent<Shahed>();
        if (shahedScript != null)
        {
            HouseManager targetManager = houseManagers[Random.Range(0, houseManagers.Count)];
            Vector3 targetPosition = targetManager.GetRandomHousePosition();

            if (targetPosition != Vector3.zero)
            {
                shahedScript.SetTarget(targetPosition, targetManager);
            }
            else
            {
                Debug.LogWarning("Target manager has no houses left.");
            }
        }
    }

    public void OnNewTargetSpawned(Transform newTarget)
    {
        Debug.LogWarning("OnNewTargetSpawned is not implemented because ShahedSpawner works with HouseManagers.");
    }

    public void UnregisterTarget(Transform target)
    {
        Debug.LogWarning("UnregisterTarget is not implemented because ShahedSpawner works with HouseManagers.");
    }

    private Transform GetRandomSpawnPoint()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.Log("No spawn points");
            return null;
        }

        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }

    private void EnableSpawning()
    {
        isSpawningEnabled = true;
        Debug.Log("Shaheds are coming!");
    }

    private void DisableSpawning()
    {
        isSpawningEnabled = false;
        Debug.Log("Shahed spawning disabled.");
    }

    public void ToggleSpawning()
    {
        (isSpawningEnabled ? (System.Action)DisableSpawning : EnableSpawning)();
    }
}