using System.Collections.Generic;
using UnityEngine;

public class ConstructionQueue : MonoBehaviour
{
    public static ConstructionQueue Instance;

    private Queue<ConstructionProcess> constructionQueue = new Queue<ConstructionProcess>();
    private List<Factory> activeFactories = new List<Factory>();
    private bool isBuilding = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterFactory(Factory factory)
    {
        if (!activeFactories.Contains(factory))
        {
            activeFactories.Add(factory);
            Debug.Log($"Factory has been register. Factory count: {activeFactories.Count}");
            
            UpdateCurrentBuildSpeed();
        }
    }

    public void UnregisterFactory(Factory factory)
    {
        if (activeFactories.Contains(factory))
        {
            activeFactories.Remove(factory);
            Debug.Log($"Factory has been destroyed, Factory count: {activeFactories.Count}");

            if (activeFactories.Count == 0)
            {
                PauseCurrentConstruction();
            }
        }
    }

    public void AddToQueue(ConstructionProcess construction)
    {
        if (construction == null || !construction.gameObject.activeInHierarchy)
        {
            Debug.LogWarning("Спроба додати недійсний об'єкт до черги.");
            return;
        }

        constructionQueue.Enqueue(construction);
        Debug.Log($"Додано до черги: {construction.gameObject.name}");

        if (!isBuilding)
        {
            StartNextConstruction();
        }
    }

    private void StartNextConstruction()
    {
        if (constructionQueue.Count > 0)
        {
            isBuilding = true;
            var nextConstruction = constructionQueue.Peek();
            nextConstruction.StartBuilding(activeFactories);
            Debug.Log($"Start construction for: {nextConstruction.gameObject.name}");
        }
        else
        {
            isBuilding = false;
            Debug.Log("Queue empty");
        }
    }


    public void NotifyConstructionComplete()
    {
        if (constructionQueue.Count > 0)
        {
            constructionQueue.Dequeue();
        }
        
        StartNextConstruction();
    }

    private void PauseCurrentConstruction()
    {
        if (constructionQueue.Count > 0)
        {
            var currentConstruction = constructionQueue.Peek();
            if (currentConstruction != null && currentConstruction.gameObject.activeInHierarchy)
            {
                currentConstruction.UpdateBuildingSpeed(0);
            }
        }
    }

    private void UpdateCurrentBuildSpeed()
    {
        if (constructionQueue.Count > 0)
        {
            var currentConstruction = constructionQueue.Peek();
            if (currentConstruction != null && currentConstruction.gameObject.activeInHierarchy)
            {
                currentConstruction.UpdateBuildingSpeed(CalculateBuildSpeed());
            }
            else
            {
                Debug.LogWarning("Поточний об'єкт будівництва недійсний. Перемикаємося.");
                NotifyConstructionComplete();
            }
        }
    }

    private float CalculateBuildSpeed()
    {
        float totalBoost = 0f;
        foreach (var factory in activeFactories)
        {
            totalBoost += factory.buildBoost;
        }

        return Mathf.Max(totalBoost, 0.5f);
    }
}
