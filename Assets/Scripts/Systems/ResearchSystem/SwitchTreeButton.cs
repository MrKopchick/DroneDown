using UnityEngine;
using UnityEngine.UI;
namespace ResearchSystem{
    public class SwitchTreeButton : MonoBehaviour
    {
        [Header("Assigned Tree")]
        public GameObject treePrefab;

        private Button button;

        private void Start()
        {
            button = GetComponent<Button>();

            if (button != null && treePrefab != null)
            {
                button.onClick.AddListener(() => ResearchTreeController.Instance.SwitchTree(treePrefab));
            }
        }
    }
}

