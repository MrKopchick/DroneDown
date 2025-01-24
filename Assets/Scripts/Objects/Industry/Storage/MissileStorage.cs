using UnityEngine;

public class MissileStorage : MonoBehaviour
{
    [Header("Storage Settings")]
    public int maxMissiles = 50;
    private int currentMissiles = 0;
    
    private void Awake()
    {
        var manager = FindObjectOfType<MissileStorageManager>();
        if (manager != null)
        {
            manager.RegisterStorage(this);
        }
        else
        {
            Debug.LogError("MissileStorage: No MissileStorageManager found in the scene.");
        }
    }


    public void AddMissiles(int count)
    {
        int addedMissiles = Mathf.Min(count, maxMissiles - currentMissiles);
        currentMissiles += addedMissiles;
        Debug.Log($"MissileStorage: Added {addedMissiles} missiles. Current: {currentMissiles}/{maxMissiles}");
    }

    public bool RemoveMissiles(int count)
    {
        if (currentMissiles >= count)
        {
            currentMissiles -= count;
            Debug.Log($"MissileStorage: Removed {count} missiles. Current: {currentMissiles}/{maxMissiles}");
            return true;
        }

        Debug.LogWarning("MissileStorage: Not enough missiles to remove!");
        return false;
    }

    public int GetCurrentMissiles() => currentMissiles;

    public int GetMaxMissiles() => maxMissiles;

    private void OnDestroy()
    {
        var storageManager = FindObjectOfType<MissileStorageManager>();
        if (storageManager != null)
        {
            storageManager.DeregisterStorage(this);
        }
    }

}