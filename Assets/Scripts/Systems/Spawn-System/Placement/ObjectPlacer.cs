using UnityEngine;
using Game.Construction;

namespace Game.Spawning.Placement
{
    public sealed class ObjectPlacer
    {
        public void PlaceObject(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            GameObject newObject = Object.Instantiate(prefab, position, rotation);
            newObject.tag = "SpawnedObject";

            var construction = newObject.GetComponent<ConstructionProcess>();
            if (construction != null)
            {
                ConstructionQueue.Instance.Enqueue(construction);
            }
        }
    }
}