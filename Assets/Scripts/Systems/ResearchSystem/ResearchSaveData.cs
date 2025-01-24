using System;
using System.Collections.Generic;

namespace ResearchSystem{
    [Serializable]
    public class ResearchSaveData
    {
        public string NodeName;
        public float Progress;
        public bool IsUnlocked;
        public bool IsActive;
        public float StartTime;
    }
}