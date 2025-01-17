using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace ResearchSystem{
    public class ResearchManager : MonoBehaviour
    {
        public static ResearchManager Instance;

        [SerializeField] private int maxConcurrentResearch = 2;
        [SerializeField] private List<ResearchTree> researchTrees;

        private readonly List<ResearchNode> activeResearches = new();

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

        public bool StartResearch(ResearchNode node)
        {
            if (activeResearches.Count >= maxConcurrentResearch || activeResearches.Contains(node) || !node.ArePrerequisitesMet())
            {
                return false;
            }

            activeResearches.Add(node);
            StartCoroutine(ResearchProgressCoroutine(node));
            return true;
        }

        public void ClearActiveResearches()
        {
            activeResearches.Clear();
        }

        private IEnumerator ResearchProgressCoroutine(ResearchNode node)
        {
            float progress = 0;

            while (progress < 1f)
            {
                progress += Time.deltaTime / node.ResearchTime;
                node.Progress = progress;
                yield return null;
            }

            CompleteResearch(node);
        }

        private void CompleteResearch(ResearchNode node)
        {
            node.Unlock();
            activeResearches.Remove(node);
            ResearchUI.Instance?.UpdateAllNodesUI();
        }
    }
}