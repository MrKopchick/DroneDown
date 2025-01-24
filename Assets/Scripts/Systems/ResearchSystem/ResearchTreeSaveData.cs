using System;
using System.Collections.Generic;

namespace ResearchSystem{
    [Serializable]
    public class ResearchTreeSaveData
    {
        public string TreeName;
        public List<ResearchSaveData> Nodes = new();
    }
}