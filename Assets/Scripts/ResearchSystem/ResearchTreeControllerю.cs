using System.Collections.Generic;
using UnityEngine;

public class ResearchTreeController : MonoBehaviour
{
    public static ResearchTreeController Instance;

    [Header("Tree Settings")]
    public Transform treeContainer;
    public List<GameObject> treePrefabs;
    private GameObject activeTree;

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

    public void SwitchTree(GameObject treePrefab)
    {
        if (activeTree != null)
        {
            Destroy(activeTree);
        }

        activeTree = Instantiate(treePrefab, treeContainer);
        Debug.Log($"open: {treePrefab.name}");
    }
}
