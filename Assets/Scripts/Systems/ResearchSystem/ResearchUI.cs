using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace ResearchSystem
{
    public class ResearchUI : MonoBehaviour
    {
        public static ResearchUI Instance;

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

        private void ResetNodeAndDependencies(ResearchNode node)
        {
            node.ResetProgress();

            foreach (var dependentNode in GetDependentNodes(node))
            {
                dependentNode.ResetProgress();
            }
        }

        private IEnumerable<ResearchNode> GetDependentNodes(ResearchNode node)
        {
            List<ResearchNode> dependentNodes = new List<ResearchNode>();

            foreach (var tree in ResearchManager.Instance.ResearchTrees)
            {
                foreach (var otherNode in tree.Nodes)
                {
                    if (otherNode.Prerequisites.Contains(node) && !dependentNodes.Contains(otherNode))
                    {
                        dependentNodes.Add(otherNode);
                        dependentNodes.AddRange(GetDependentNodes(otherNode));
                    }
                }
            }

            return dependentNodes;
        }

        public void UpdateAllNodesUI()
        {
            foreach (var nodeUI in FindObjectsOfType<ResearchNodeUI>())
            {
                nodeUI.RefreshUI();
            }
        }
        
        public void OnUIOpened()
        {
            RefreshAllNodes();
        }

        private void RefreshAllNodes()
        {
            foreach (var nodeUI in FindObjectsOfType<ResearchNodeUI>())
            {
                nodeUI.RefreshUI();
            }
        }

    }
}