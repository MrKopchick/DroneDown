using UnityEngine;

public class SpawnableObjectInstance : MonoBehaviour
{
    [Header("Object Settings")]
    public SpawnableObject spawnableObject;

    public bool IsTarget => spawnableObject != null && spawnableObject.isTarget;

    private void Awake()
    {
        if (spawnableObject == null)
        {
            Debug.LogError($"SpawnableObject не встановлено на {gameObject.name}!");
        }

        if (IsTarget)
        {
            ShahedSpawner spawner = FindObjectOfType<ShahedSpawner>();
            if (spawner != null)
            {
                spawner.OnNewTargetSpawned(transform);
            }
        }
    }

    private void OnDestroy()
    {
        if (IsTarget)
        {
            ShahedSpawner spawner = FindObjectOfType<ShahedSpawner>();
            if (spawner != null)
            {
                spawner.UnregisterTarget(transform);
            }
        }
    }
}