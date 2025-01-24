using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace RadarSystem{
    public class Radar : IUpdatable
    {
        public Vector3 Position { get; }
        public float DetectionRadius { get; }
        public float UpdateFrequency { get; }
        private float lastUpdateTime;

        public List<Target> DetectedTargets { get; private set; }

        public Radar(Vector3 position, float detectionRadius, float updateFrequency)
        {
            Position = position;
            DetectionRadius = detectionRadius;
            UpdateFrequency = updateFrequency;
            DetectedTargets = new List<Target>();
        }

        public void Update(float deltaTime)
        {
            lastUpdateTime += deltaTime;

            if (lastUpdateTime >= UpdateFrequency)
            {
                ScanForTargets();
                lastUpdateTime = 0;
            }
        }


        private void ScanForTargets()
        {
            DetectedTargets.Clear();
           // var allTargets = TargetManager.Instance.GetAllTargets();

            /*foreach (var target in allTargets)
            {
                if (Vector3.Distance(Position, target.Position) <= DetectionRadius)
                {
                    target.IsDetected = true;
                    DetectedTargets.Add(target);
                }
            }*/
        }
    }
}
