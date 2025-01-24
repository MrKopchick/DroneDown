using UnityEngine;
using System.Collections.Generic;

namespace ResearchSystem
{
    [CreateAssetMenu(fileName = "ResearchNode", menuName = "Research/Research Node")]
    public class ResearchNode : ScriptableObject
    {
        [SerializeField]
        private string nodeName;
        [SerializeField]
        private float researchTime;
        [SerializeField]
        private List<ResearchNode> prerequisites;
        [SerializeField]
        private ResearchTree tree;

        private bool isUnlocked;
        private float progress;
        private float startTime;

        public string NodeName => nodeName;
        public float ResearchTime => researchTime;
        public bool IsUnlocked => isUnlocked;
        public float Progress { get => progress; set => progress = Mathf.Clamp01(value); }
        public float StartTime { get => startTime; set => startTime = value; }
        public List<ResearchNode> Prerequisites => prerequisites;
        public ResearchTree Tree => tree;

        public void Unlock()
        {
            isUnlocked = true;
            progress = 1f;
        }

        public void ResetProgress()
        {
            isUnlocked = false;
            progress = 0f;
            startTime = 0f;
        }

        public bool ArePrerequisitesMet() => prerequisites.TrueForAll(prerequisite => prerequisite.IsUnlocked);
    }
}
