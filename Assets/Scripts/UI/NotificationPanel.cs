using UnityEngine;
using TMPro;
using DG.Tweening;

public class NotificationPanel : MonoBehaviour
{
    [SerializeField] private GameObject panelPrefab;
    [SerializeField] private Transform parentCanvas;
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float displayDuration = 2f;

    public void ShowMessage(string message, Color color)
    {

        GameObject panelInstance = Instantiate(panelPrefab, parentCanvas);
        RectTransform panelTransform = panelInstance.GetComponent<RectTransform>();
        TMP_Text messageText = panelInstance.GetComponentInChildren<TMP_Text>();

        if (messageText == null)
        {
            Debug.LogError("NotificationPanel: TMP_Text не знайдений у префабі!");
            Destroy(panelInstance);
            return;
        }
        
        messageText.text = message;
        messageText.color = color;
        
        Vector2 hiddenPosition = new Vector2(0, -200);
        Vector2 visiblePosition = new Vector2(0, 0);
        panelTransform.anchoredPosition = hiddenPosition;
        
        panelTransform.DOAnchorPos(visiblePosition, animationDuration).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            Invoke(nameof(HidePanel), displayDuration);
        });

        void HidePanel()
        {
            panelTransform.DOAnchorPos(hiddenPosition, animationDuration).SetEase(Ease.InQuad).OnComplete(() =>
            {
                Destroy(panelInstance); 
            });
        }
    }
}