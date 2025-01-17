using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class MenuAnimation : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private RectTransform gameTitle; // Назва гри
    [SerializeField] private TextMeshProUGUI gameTitleText; // Текст назви гри
    [SerializeField] private RectTransform[] buttons; // Кнопки (Play, Settings, Exit)
    [SerializeField] private Image fadeImage; // Чорний екран

    [Header("Animation Settings")]
    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private float titleMoveDuration = 1f; // Тривалість руху назви
    [SerializeField] private float buttonsAppearDelay = 0.5f; // Затримка між появами кнопок
    [SerializeField] private float buttonsMoveDuration = 1f; // Тривалість руху кнопок
    [SerializeField] private float colorChangeDuration = 1f; // Тривалість зміни кольору

    [Header("Button Positions")]
    [SerializeField] private Vector2[] buttonTargetPositions; // Цільові позиції кнопок

    private void Start()
    {
        // Перевірка кількості кнопок і позицій
        if (buttons.Length != buttonTargetPositions.Length)
        {
            Debug.LogError("Кількість кнопок і позицій не збігається!");
            return;
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            RectTransform button = buttons[i];
            button.GetComponent<CanvasGroup>().alpha = 0;
        }
        StartFadeIn();
    }

    private void StartFadeIn()
    {
        fadeImage.DOFade(0, fadeDuration).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            Destroy(fadeImage.gameObject);
            AnimateTitle();
        });
    }

    private void AnimateTitle()
    {
        gameTitle.DOAnchorPosY(gameTitle.anchoredPosition.y + 200, titleMoveDuration).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            ShowButtons();
        });
    }

    private void ShowButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            RectTransform button = buttons[i];
            button.anchoredPosition = new Vector2(buttonTargetPositions[i].x, gameTitle.anchoredPosition.y - 100);
            button.GetComponent<CanvasGroup>().alpha = 0;

            button.DOAnchorPos(buttonTargetPositions[i], buttonsMoveDuration).SetEase(Ease.OutQuad).SetDelay(i * buttonsAppearDelay);
            button.GetComponent<CanvasGroup>().DOFade(1, buttonsMoveDuration).SetDelay(i * buttonsAppearDelay);
        }

        DOVirtual.DelayedCall(buttonsAppearDelay * buttons.Length + buttonsMoveDuration, ChangeLastCharacterColor);
    }

    private void ChangeLastCharacterColor()
    {
        string text = gameTitleText.text;
        if (string.IsNullOrEmpty(text)) return;

        int lastIndex = text.Length - 1;
        string newText = $"{text.Substring(0, lastIndex)}<color=#FFA0A0>{text[lastIndex]}</color>";
        gameTitleText.SetText(newText);
    }
}
