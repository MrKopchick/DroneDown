using UnityEngine;
using UnityEngine.UI;

namespace ResearchSystem
{
    public class ResetResearchButton : MonoBehaviour
    {
        [SerializeField] private Button resetButton;

        private void Start()
        {
            resetButton.onClick.AddListener(ResetResearchProgress);
        }

        private void ResetResearchProgress()
        {
            if (ResearchManager.Instance != null)
            {
                Debug.Log("Reset button clicked. Attempting to reset progress...");
                ResearchCore.Instance.ResetProgress();
            }
            else
            {
                Debug.LogWarning("ResearchManager instance is not available. Cannot reset progress.");
            }
        }
    }
}
