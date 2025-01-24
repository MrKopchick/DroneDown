using UnityEngine;
using System.Collections.Generic;

public class ConstructionProcess : MonoBehaviour
{
    [SerializeField] private float baseConstructionTime = 10f;
    private float currentConstructionTime;
    private bool isUnderConstruction = false;
    private float buildSpeed = 1f;

    private void Awake()
    {
        var spawnableObject = GetComponent<GenericIDCard>()?.spawnableObject;
        if (spawnableObject != null)
        {
            baseConstructionTime = spawnableObject.baseConstructionTime;
        }
        else
        {
            Debug.LogError($"SpawnableObject null on {gameObject.name}");
        }

        currentConstructionTime = baseConstructionTime;
    }

    public float ConstructionProgress => Mathf.Clamp01(1f - (currentConstructionTime / baseConstructionTime));

    public void StartBuilding(List<Factory> factories = null)
    {
        isUnderConstruction = true;

        if (factories != null)
        {
            float totalBoost = 0f;
            foreach (var factory in factories)
            {
                totalBoost += factory.buildBoost;
            }
            buildSpeed = Mathf.Max(1f, totalBoost);
        }

        Debug.Log($"Build process started for {gameObject.name} time: {baseConstructionTime}");
    }


    public void PauseBuilding()
    {
        isUnderConstruction = false;
        Debug.Log($"Build stop for: {gameObject.name}.");
    }

    public void CompleteBuilding()
    {
        isUnderConstruction = false;
        currentConstructionTime = 0f;
        Debug.Log($"Build complete: {gameObject.name}.");
    }

    public void UpdateBuildingSpeed(float newSpeed)
    {
        buildSpeed = newSpeed;
    }

    private void Update()
    {
        if (!isUnderConstruction) return;

        currentConstructionTime -= Time.deltaTime * buildSpeed;

        if (currentConstructionTime <= 0)
        {
            currentConstructionTime = 0f;
            CompleteBuilding();
            ConstructionQueue.Instance.NotifyConstructionComplete();
        }
    }
}