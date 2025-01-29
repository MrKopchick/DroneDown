using UnityEngine;
using Game.Construction;
using Game.Spawning.Data;
public class GenericIDCard : MonoBehaviour
{
    public SpawnableObject spawnableObject;
    private ConstructionProcess constructionProcess;

    private void Awake()
    {
        constructionProcess = GetComponent<ConstructionProcess>();

        if (spawnableObject == null)
        {
            Debug.LogError($"SpawnableObject null on {gameObject.name}!");
        }

        if (constructionProcess == null)
        {
            Debug.LogError($"ConstructionProcess null on {gameObject.name}!");
        }
    }

    public string GetIDCardContent()
    {
        if (spawnableObject == null) return "No data available.";

        if (constructionProcess != null && constructionProcess.Progress < 1f)
        {
            return $"Name: {spawnableObject.objectName}\n" +
                   $"Construction Progress: {Mathf.RoundToInt(constructionProcess.Progress * 100)}%";
        }

        return $"Name: {spawnableObject.objectName}\n" +
               $"Status: Fully Operational";
    }
}