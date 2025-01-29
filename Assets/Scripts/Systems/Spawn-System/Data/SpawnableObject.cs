using UnityEngine;
namespace Game.Spawning.Data
{
    [CreateAssetMenu(fileName = "SpawnableObject", menuName = "Spawn System/Spawnable Object")]
    public class SpawnableObject : ScriptableObject
    {
        [Header("General Settings")]
        public string objectName;
        public GameObject prefab;
        public GameObject previewPrefab;

        [Header("Construction Settings")]
        public bool requiresConstruction;
        public float baseConstructionTime = 10f;
        public float constructionSpeedMultiplier = 1f;

        [Header("Target Settings")]
        public bool isTarget;

        [Header("Economic Settings")]
        public int price;
    }
}