using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class IDCardManager : MonoBehaviour
{
    [Header("Panel Animation")]
    [SerializeField] private RectTransform panel;
    [SerializeField] private float animationDuration = 0.3f;
    [SerializeField] private Vector2 hiddenPosition = new Vector2(0, -Screen.height);
    [SerializeField] private Vector2 visiblePosition = new Vector2(0, 0);

    [Header("UI Components")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text contentText;
    [SerializeField] private Button closeButton;

    private IDCardBase currentIDCard;
    private bool isPanelOpen = false;

    public static IDCardManager Instance;

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
        panel.anchoredPosition = hiddenPosition;

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseButtonPressed);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HideIDCard();
        }

        if (isPanelOpen && currentIDCard != null)
        {
            UpdateIDCardContent();
        }
    }

    public void ShowIDCard(IDCardBase idCard)
    {
        if (idCard == null)
        {
            Debug.LogError("Provided ID card is null!");
            return;
        }

        if (CityPanelUI.Instance != null && CityPanelUI.Instance.IsPanelOpen)
        {
            CityPanelUI.Instance.HideCityPanel(() => OpenPanel(idCard));
        }
        else if (currentIDCard == idCard && isPanelOpen)
        {
            Debug.Log("Panel is already open for this object.");
            return;
        }
        else
        {
            GameManager.Instance.isPauseReady = false;
            OpenPanel(idCard);
        }
    }

    public void ShowCityCard(CityIdCard cityCard)
    {
        if (cityCard == null)
        {
            Debug.LogError("Provided City ID card is null!");
            return;
        }

        if (CityPanelUI.Instance != null)
        {
            CityPanelUI.Instance.ShowCityPanel(cityCard.GetComponent<CityStats>());
            return;
        }

        nameText.text = cityCard.name;
        contentText.text = cityCard.GetCityCardContent();

        panel.DOAnchorPos(visiblePosition, animationDuration).SetEase(Ease.OutQuad);
        isPanelOpen = true;
    }

    private void OpenPanel(IDCardBase idCard)
    {
        currentIDCard = idCard;

        UpdateIDCardContent();

        panel.DOAnchorPos(visiblePosition, animationDuration).SetEase(Ease.OutQuad);
        isPanelOpen = true;
    }

    private void UpdateIDCardContent()
    {
        if (currentIDCard == null) return;

        nameText.text = currentIDCard.ObjectName;
        contentText.text = currentIDCard.GetIDCardContent();
    }

    public void HideIDCard(TweenCallback onComplete = null)
    {
        if (!isPanelOpen) return;

        panel.DOAnchorPos(hiddenPosition, animationDuration).SetEase(Ease.InQuad).OnComplete(() =>
        {
            currentIDCard = null;
            isPanelOpen = false;
            onComplete?.Invoke();
            GameManager.Instance.isPauseReady = true;
        });
    }

    public void RestorePanel()
    {
        if (!isPanelOpen && currentIDCard != null)
        {
            UpdateIDCardContent();
            panel.DOAnchorPos(visiblePosition, animationDuration).SetEase(Ease.OutQuad);
            isPanelOpen = true;
        }
    }

    public void CloseButtonPressed()
    {
        HideIDCard();
    }

    public bool IsPanelOpen => isPanelOpen;
}
