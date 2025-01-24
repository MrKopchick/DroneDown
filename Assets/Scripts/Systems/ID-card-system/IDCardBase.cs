using UnityEngine;

public abstract class IDCardBase : MonoBehaviour
{
    [SerializeField] protected SpawnableObject spawnableObject;

    protected virtual void Awake()
    {
        if (spawnableObject == null)
        {
            Debug.LogError($"no spawnableObject on{gameObject.name}!");
        }
    }

    public string ObjectName => spawnableObject != null ? spawnableObject.objectName : "Unknown";
    public string ObjectType => spawnableObject != null ? spawnableObject.GetType().Name : "Unknown";

    public abstract string GetSpecificContent();

    public string GetIDCardContent()
    {
        var constructionProcess = GetComponent<ConstructionProcess>();

        if (constructionProcess != null && constructionProcess.ConstructionProgress < 1f)
        {
            return $"Name: {ObjectName}\n" +
                   $"Construction Progress: {Mathf.RoundToInt(constructionProcess.ConstructionProgress * 100)}%";
        }

        string baseContent = $"Name: {ObjectName}\n";
        return baseContent + GetSpecificContent();
    }
}