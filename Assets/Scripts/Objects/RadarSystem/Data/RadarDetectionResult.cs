using UnityEngine;
using System.Collections.Generic;

namespace RadarSystem{
    namespace Data
    {
        public struct RadarDetectionResult
        {
            public Vector3 RadarPosition;
            public List<RadarTarget> DetectedTargets;
        }
    }
}