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
            Debug.LogError($"SpawnableObject is not set on {gameObject.name}!");
        }

        if (IsTarget)
        {
            ShahedSpawnAI spawner = FindObjectOfType<ShahedSpawnAI>();
            spawner?.RegisterTarget(transform);
        }
    }

    private void OnDestroy()
    {
        if (IsTarget)
        {
            ShahedSpawnAI spawner = FindObjectOfType<ShahedSpawnAI>();
            spawner?.UnregisterTarget(transform);
        }
    }
}
