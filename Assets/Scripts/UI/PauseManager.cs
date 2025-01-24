using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace UI
{
    public class PauseManager : MonoBehaviour
    {
        [SerializeField] private GameObject pauseWindowPrefab;
        [SerializeField] private Transform uiParent;
        [SerializeField] private PostProcessVolume PostProcessCamera;

        private bool isPaused = false;
        private DepthOfField depthOfField;
        private GameObject TopPanel;

        private void Start()
        {
            TopPanel = GameObject.Find("TopPanel");
            PostProcessCamera = GameObject.Find("MainCamera").GetComponent<PostProcessVolume>();

            if (PostProcessCamera.profile.TryGetSettings(out depthOfField))
            {
                depthOfField.focusDistance.value = 8f; 
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.isPauseReady)
            {
                if (!isPaused)
                {
                    Pause();
                }
                else
                {
                    Resume();
                }
            }
        }

        private void Pause()
        {
            Time.timeScale = 0;
            Instantiate(pauseWindowPrefab, uiParent);
            TopPanel.SetActive(false);
            isPaused = true;
            depthOfField.focusDistance.value = 2f;
        }

        private void Resume()
        {
            Time.timeScale = 1;
            Destroy(GameObject.Find("PauseWindow(Clone)"));
            isPaused = false;
            TopPanel.SetActive(true);
            depthOfField.focusDistance.value = 8f;
        }
    }
}
