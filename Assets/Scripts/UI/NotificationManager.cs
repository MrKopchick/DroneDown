using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;

public class NotificationManager : MonoBehaviour
{
    [SerializeField] private GameObject panelPrefab;
    [SerializeField] private Transform parentCanvas;
    [SerializeField] private float animationDuration = 0.4f;
    [SerializeField] private float displayDuration = 3f;

    [Header("Message Colors")]
    [SerializeField] private Color whiteColor = Color.white;
    [SerializeField] private Color redColor = Color.red;
    [SerializeField] private Color yellowColor = Color.yellow;
    [SerializeField] private Color greenColor = Color.green;

    public static NotificationManager Instance { get; private set; }

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

    public void ShowMessage(string message, string color)
    {
        if (panelPrefab == null || parentCanvas == null)
            return;

        var panelInstance = Instantiate(panelPrefab, parentCanvas);
        var messageText = panelInstance.GetComponentInChildren<TMP_Text>();
        if (messageText == null)
        {
            Destroy(panelInstance);
            return;
        }

        messageText.text = message;
        messageText.color = GetColorByName(color);

        AnimateShow(panelInstance);
        StartCoroutine(HidePanelAfterDelay(panelInstance));
    }

    private void AnimateShow(GameObject panelInstance)
    {
        var panelTransform = panelInstance.GetComponent<RectTransform>();
        if (panelTransform == null)
        {
            Destroy(panelInstance);
            return;
        }

        panelTransform.anchoredPosition = new Vector2(0, -200);
        panelTransform.DOAnchorPosY(0, animationDuration)
            .SetEase(Ease.OutQuad);
    }

    private IEnumerator HidePanelAfterDelay(GameObject panelInstance)
    {
        yield return new WaitForSeconds(displayDuration);
        AnimateHide(panelInstance);
    }

    private void AnimateHide(GameObject panelInstance)
    {
        var panelTransform = panelInstance.GetComponent<RectTransform>();
        if (panelTransform != null)
        {
            panelTransform.DOAnchorPosY(-200, animationDuration)
                .SetEase(Ease.InQuad)
                .OnComplete(() => Destroy(panelInstance));
        }
        else
        {
            Destroy(panelInstance);
        }
    }

    private Color GetColorByName(string color)
    {
        return color.ToLower() switch
        {
            "white" => whiteColor,
            "red" => redColor,
            "yellow" => yellowColor,
            "green" => greenColor,
            _ => whiteColor,
        };
    }
}
