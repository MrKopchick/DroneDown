using UnityEngine;
using System.Collections.Generic;

namespace RadarSystem{
      
    namespace Data{
        public class ProcessedRadarData
        {
            public readonly List<RadarTarget> ValidTargets;
            public readonly Vector3 RadarPosition;

            public ProcessedRadarData(RadarDetectionResult result)
            {
                RadarPosition = result.RadarPosition;
                ValidTargets = result.DetectedTargets;
            }
        }
    }
}
