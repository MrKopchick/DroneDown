using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OpenPanelScript : MonoBehaviour
{
    [System.Serializable]
    public class ButtonPanelPair
    {
        public Button button; 
        public RectTransform panelPrefab; 
    }

    public List<ButtonPanelPair> buttonPanelPairs; 
    public Transform spawnPoint; 
    public Transform finalPoint; 
    public float animationDuration = 0.5f; 

    private RectTransform currentPanel;
    private Button currentButton; 

    private void Start()
    {
        foreach (var pair in buttonPanelPairs)
        {
            pair.button.onClick.AddListener(() => OnButtonClicked(pair));
        }
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CameraShake.Instance.Shake(0.015f, 0.15f);
            CloseCurrentPanel();
        }
    }

    private void OnButtonClicked(ButtonPanelPair pair)
    {
        if (currentPanel != null)
        {
            if (currentButton == pair.button)
            {
                CloseCurrentPanel();
                return;
            }
            CloseCurrentPanel(() => OpenPanel(pair));
        }
        else
        {
            OpenPanel(pair);
        }
    }

    private void OpenPanel(ButtonPanelPair pair)
    {
        currentButton = pair.button;
        
        currentPanel = Instantiate(pair.panelPrefab, spawnPoint.position, Quaternion.identity, spawnPoint.parent);
        currentPanel.anchoredPosition = spawnPoint.GetComponent<RectTransform>().anchoredPosition;
        
        currentPanel.DOAnchorPos(finalPoint.GetComponent<RectTransform>().anchoredPosition, animationDuration)
            .SetEase(Ease.OutQuad);
    }

    private void CloseCurrentPanel(System.Action onComplete = null)
    {
        if (currentPanel == null) return;
        
        currentPanel.DOAnchorPos(spawnPoint.GetComponent<RectTransform>().anchoredPosition, animationDuration)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                Destroy(currentPanel.gameObject);
                currentPanel = null;
                currentButton = null;
                onComplete?.Invoke();
            });
    }
}
