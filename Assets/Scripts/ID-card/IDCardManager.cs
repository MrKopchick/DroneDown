using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;
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

        if (currentIDCard == idCard && isPanelOpen)
        {
            Debug.Log("Panel is already open for this object.");
            return;
        }

        if (isPanelOpen)
        {
            HideIDCard(() => OpenPanel(idCard));
        }
        else
        {
            OpenPanel(idCard);
        }
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
        nameText.text = currentIDCard.ObjectName;
        contentText.text = currentIDCard.GetIDCardContent();

        /*if (currentIDCard is IDCardBase spawnable && spawnable is not null)
        {
            contentText.text += $"\nType: {spawnable.GetType().Name}";
        }*/
    }

    public void ShowCityCard(CityIdCard cityCard)
    {
        if (cityCard == null)
        {
            Debug.LogError("Provided City ID card is null!");
            return;
        }

        nameText.text = cityCard.name;
        contentText.text = cityCard.GetCityCardContent();

        panel.DOAnchorPos(visiblePosition, animationDuration).SetEase(Ease.OutQuad);
        isPanelOpen = true;
    }

    public void HideIDCard(TweenCallback onComplete = null)
    {
        if (!isPanelOpen) return;

        panel.DOAnchorPos(hiddenPosition, animationDuration).SetEase(Ease.InQuad).OnComplete(() =>
        {
            currentIDCard = null;
            isPanelOpen = false;
            onComplete?.Invoke();
        });
    }

    public void CloseButtonPressed()
    {
        HideIDCard();
    }
}
