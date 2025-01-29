using System.Collections.Generic;

namespace RadarSystem{
    namespace Data{
        [System.Serializable]
        public class RadarSettings
        {
            public float Range = 10f;
            public float RefreshRate = 1f;
            public float RotationSpeed = 10f;
            public float RotationAngle = 90f;
            public float HeightRange = 50f;
            public List<string> TargetTags = new List<string>();
        }
    }
}