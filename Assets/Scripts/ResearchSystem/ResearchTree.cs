using UnityEngine;
using System.Collections.Generic;
namespace ResearchSystem{
    public class ResearchTree : MonoBehaviour
    {
        [SerializeField] private string treeName;
        [SerializeField] private List<ResearchNode> nodes;

        public List<ResearchNode> Nodes => nodes;
    }
}
