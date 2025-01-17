using UnityEngine;
using System.Collections.Generic;

namespace ResearchSystem
{
    [CreateAssetMenu(fileName = "ResearchNode", menuName = "Research/Research Node")]
    public class ResearchNode : ScriptableObject
    {
        [SerializeField] private string nodeName;
        [SerializeField] private string description;
        [SerializeField] private Sprite icon;
        [SerializeField] private float researchTime;
        [SerializeField] private List<ResearchNode> prerequisites;

        private bool isUnlocked;
        private float progress;

        public string NodeName => nodeName;
        public string Description => description;
        public Sprite Icon => icon;
        public float ResearchTime => researchTime;
        public bool IsUnlocked => isUnlocked;
        public List<ResearchNode> Prerequisites => prerequisites;

        public float Progress
        {
            get => progress;
            set => progress = Mathf.Clamp01(value);
        }

        public void Unlock()
        {
            isUnlocked = true;
            progress = 1f;
        }

        public void ResetProgress()
        {
            isUnlocked = false;
            progress = 0f;
        }

        public bool ArePrerequisitesMet()
        {
            return prerequisites.TrueForAll(prerequisite => prerequisite.isUnlocked);
        }
    }
}
