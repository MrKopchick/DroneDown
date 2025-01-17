using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace ResearchSystem
{
    public class ResearchWindowController : MonoBehaviour
    {
        [SerializeField] private GameObject researchWindowPrefab;
        [SerializeField] private Transform uiParent;
        [SerializeField] private float animationDuration = 0.4f;
        [SerializeField] private Vector3 closedScale = new Vector3(0.8f, 0.8f, 0.8f);
        [SerializeField] private Vector3 openedScale = Vector3.one;
        [SerializeField] private float fadeDuration = 0.2f;
        [SerializeField] private GameObject topPanel;

        private GameObject currentWindow;
        private CanvasGroup canvasGroup;
        private bool isWindowOpen;

        private void Update()
        {
            if (isWindowOpen && Input.GetKeyDown(KeyCode.Escape))
            {
                CloseResearchWindow();
            }
        }

        public void OpenResearchWindow()
        {
            if (currentWindow == null)
            {
                currentWindow = Instantiate(researchWindowPrefab, uiParent);
                canvasGroup = currentWindow.AddComponent<CanvasGroup>();
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                currentWindow.transform.localScale = closedScale;

                Button closeButton = currentWindow.GetComponentInChildren<Button>();
                if (closeButton != null)
                {
                    closeButton.onClick.AddListener(CloseResearchWindow);
                }

                CameraController.IsInputBlocked = true;

                if (topPanel != null)
                {
                    topPanel.SetActive(false);
                }
            }

            canvasGroup.DOFade(1f, fadeDuration).OnStart(() =>
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            });

            currentWindow.transform.DOScale(openedScale, animationDuration).SetEase(Ease.OutQuad);
            isWindowOpen = true;
        }

        public void CloseResearchWindow()
        {
            if (topPanel != null) topPanel.SetActive(true);
        
            if (currentWindow != null)
            {
                canvasGroup.DOFade(0f, fadeDuration).OnComplete(() =>
                {
                    canvasGroup.interactable = false;
                    canvasGroup.blocksRaycasts = false;
                });

                currentWindow.transform.DOScale(closedScale, animationDuration).SetEase(Ease.InQuad).OnComplete(() =>
                {
                    Destroy(currentWindow);
                    currentWindow = null;
                    isWindowOpen = false;
                    CameraController.IsInputBlocked = false;
                });
            }
        }
    }
}
