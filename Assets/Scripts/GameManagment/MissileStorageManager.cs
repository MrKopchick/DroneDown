using System.Collections.Generic;
using UnityEngine;

public class MissileStorageManager : MonoBehaviour
{
    private Queue<MissileStorage> storageQueue = new Queue<MissileStorage>();
    
    public void RegisterStorage(MissileStorage storage)
    {
        if (!storageQueue.Contains(storage))
        {
            storageQueue.Enqueue(storage);
            Debug.Log($"MissileStorageManager: Registered new storage {storage.name}");
        }
        else
        {
            Debug.Log($"MissileStorageManager: Storage {storage.name} is already registered.");
        }
    }

    public void DeregisterStorage(MissileStorage storage)
    {
        if (storageQueue.Contains(storage))
        {
            var tempQueue = new Queue<MissileStorage>(storageQueue);
            storageQueue.Clear();

            foreach (var item in tempQueue)
            {
                if (item != storage)
                {
                    storageQueue.Enqueue(item);
                }
            }
            Debug.Log($"Deregistered storage {storage.name}");
        }
    }

    public MissileStorage GetNextStorage()
    {
        if (storageQueue.Count == 0)
        {
            Debug.LogWarning("No available storages");
            return null;
        }

        var nextStorage = storageQueue.Peek();
        if (nextStorage.GetCurrentMissiles() >= nextStorage.GetMaxMissiles())
        {
            storageQueue.Enqueue(storageQueue.Dequeue());
            return GetNextStorage();
        }

        return nextStorage;
    }
}