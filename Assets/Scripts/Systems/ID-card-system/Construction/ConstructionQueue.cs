using System.Collections.Generic;
using UnityEngine;

namespace Game.Construction
{
    public sealed class ConstructionQueue : MonoBehaviour
    {
        public static ConstructionQueue Instance { get; private set; }

        private readonly Queue<ConstructionProcess> queue = new();
        private readonly List<Factory> factories = new();
        private bool isBuilding;

        private void Awake() => InitializeSingleton();

        private void InitializeSingleton()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void RegisterFactory(Factory factory)
        {
            if (!factories.Contains(factory))
            {
                factories.Add(factory);
                UpdateConstructionSpeed();
            }
        }

        public void UnregisterFactory(Factory factory)
        {
            if (factories.Remove(factory))
            {
                UpdateConstructionSpeed();
            }
        }

        public void Enqueue(ConstructionProcess process)
        {
            if (process == null) return;
            
            queue.Enqueue(process);
            if (!isBuilding) StartNextProcess();
        }

        private void StartNextProcess()
        {
            if (queue.Count == 0)
            {
                isBuilding = false;
                return;
            }

            isBuilding = true;
            var process = queue.Peek();
            process.StartBuilding(CalculateBuildSpeed());
        }

        public void NotifyComplete()
        {
            if (queue.Count > 0) queue.Dequeue();
            StartNextProcess();
        }

        private void UpdateConstructionSpeed()
        {
            if (queue.Count > 0 && queue.Peek() is { } process)
            {
                process.UpdateBuildingSpeed(CalculateBuildSpeed());
            }
        }

        private float CalculateBuildSpeed()
        {
            float speed = 1f;
            factories.ForEach(f => speed += f.buildBoost);
            return Mathf.Max(speed, 0.5f);
        }
    }
}