using System.Collections.Generic;
using System;
using UnityEngine;

namespace ResearchSystem
{
    public class ResearchCore : MonoBehaviour
    {
        public static ResearchCore Instance;

        private readonly List<ResearchNode> activeResearches = new();
        private const string SaveFileName = "ResearchProgress.json";
        private bool saveRequired;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadProgress();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            UpdateActiveResearches();
            if (saveRequired)
            {
                SaveProgress();
                saveRequired = false;
            }
        }

        public bool StartResearch(ResearchNode node)
        {
            if (activeResearches.Contains(node) || !node.ArePrerequisitesMet() || IsNodeResearchingInSameTree(node))
            {
                return false;
            }

            activeResearches.Add(node);
            node.StartTime = Time.time;
            saveRequired = true;
            return true;
        }

        public bool IsNodeActive(ResearchNode node) => activeResearches.Contains(node);

        private bool IsNodeResearchingInSameTree(ResearchNode node)
        {
            return activeResearches.Exists(activeNode => activeNode.Tree == node.Tree);
        }

        public ResearchNode FindNodeByName(string nodeName)
        {
            foreach (var tree in ResearchManager.Instance.ResearchTrees)
            {
                foreach (var node in tree.Nodes)
                {
                    if (node.NodeName == nodeName)
                    {
                        return node;
                    }
                }
            }
            return null;
        }

        private void UpdateActiveResearches()
        {
            float currentTime = Time.time;
            foreach (var node in activeResearches.ToArray())
            {
                if (!node.IsUnlocked)
                {
                    float elapsedTime = currentTime - node.StartTime;
                    node.Progress = Mathf.Clamp01(elapsedTime / node.ResearchTime);

                    if (node.Progress >= 1f)
                    {
                        CompleteResearch(node);
                    }
                }
            }
        }

        private void CompleteResearch(ResearchNode node)
        {
            node.Unlock();
            activeResearches.Remove(node);
            saveRequired = true;
        }

        public void SaveProgress()
        {
            var saveData = new List<ResearchSaveData>();
            foreach (var tree in ResearchManager.Instance.ResearchTrees)
            {
                foreach (var node in tree.Nodes)
                {
                    saveData.Add(new ResearchSaveData
                    {
                        NodeName = node.NodeName,
                        Progress = node.Progress,
                        StartTime = node.StartTime,
                        IsUnlocked = node.IsUnlocked,
                        IsActive = activeResearches.Contains(node)
                    });
                }
            }
            SaveSystem.Save(saveData, SaveFileName);
        }

        public void LoadProgress()
        {
            var saveData = SaveSystem.Load<List<ResearchSaveData>>(SaveFileName);
            if (saveData == null) return;

            float elapsedTime = GetElapsedTime();
            foreach (var nodeSave in saveData)
            {
                ResearchNode node = FindNodeByName(nodeSave.NodeName);
                if (node == null) continue;

                node.Progress = nodeSave.Progress;

                if (nodeSave.IsUnlocked)
                {
                    node.Unlock();
                }
                else if (nodeSave.IsActive)
                {
                    float additionalTime = elapsedTime - (node.ResearchTime * node.Progress);
                    if (additionalTime >= node.ResearchTime * (1f - node.Progress))
                    {
                        node.Unlock();
                    }
                    else
                    {
                        node.Progress += additionalTime / node.ResearchTime;
                        node.StartTime = Time.time - (node.ResearchTime * node.Progress);
                        activeResearches.Add(node);
                    }
                }
            }
        }
        
        private float GetElapsedTime()
        {
            if (!PlayerPrefs.HasKey("LastSaveTime")) return 0f;
            double lastSaveTime = double.Parse(PlayerPrefs.GetString("LastSaveTime"));
            double currentTime = DateTime.Now.ToOADate();
            return (float)(currentTime - lastSaveTime) * 86400f;
        }

        public void ResetProgress()
        {
            foreach (var tree in ResearchManager.Instance.ResearchTrees)
            {
                foreach (var node in tree.Nodes)
                {
                    node.ResetProgress();
                }
            }
            activeResearches.Clear();
            saveRequired = true;
        }
    }
}
