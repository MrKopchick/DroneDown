using UnityEngine;
using System.Collections.Generic;

namespace ResearchSystem
{
    public class ResearchManager : MonoBehaviour
    {
        public static ResearchManager Instance;

        [SerializeField]
        private List<ResearchTree> researchTrees;
        public List<ResearchTree> ResearchTrees => researchTrees;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public bool StartResearch(ResearchNode node) => ResearchCore.Instance.StartResearch(node);
        public bool IsNodeActive(ResearchNode node) => ResearchCore.Instance.IsNodeActive(node);
        public void ResetProgress() => ResearchCore.Instance.ResetProgress();
    }
}
