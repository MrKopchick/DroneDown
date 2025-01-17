using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class HoverUnderlineEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Underline Settings")]
    [SerializeField] private RectTransform underline; // Панелька/риса під кнопкою
    [SerializeField] private float animationDuration = 0.3f; // Тривалість анімації
    [SerializeField] private float targetWidthMultiplier = 1.2f; // У відсотках від ширини кнопки
    [SerializeField] private Color hoverColor = Color.white; // Колір риски при наведенні
    [SerializeField] private Color normalColor = Color.clear; // Колір риски в нормальному стані

    private float buttonWidth;
    private Image underlineImage;

    private void Start()
    {
        if (underline == null)
        {
            Debug.LogError("Underline RectTransform не заданий!");
            return;
        }

        underlineImage = underline.GetComponent<Image>();
        if (underlineImage == null)
        {
            Debug.LogError("Риса повинна мати компонент Image!");
            return;
        }

        buttonWidth = GetComponent<RectTransform>().rect.width;
        ResetUnderline();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AnimateUnderline(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        AnimateUnderline(false);
    }

    private void AnimateUnderline(bool isHover)
    {
        float targetWidth = isHover ? buttonWidth * targetWidthMultiplier : 0;
        Color targetColor = isHover ? hoverColor : normalColor;

        underline.DOSizeDelta(new Vector2(targetWidth, underline.sizeDelta.y), animationDuration).SetEase(Ease.OutQuad);
        underlineImage.DOColor(targetColor, animationDuration).SetEase(Ease.OutQuad);
    }

    private void ResetUnderline()
    {
        underline.sizeDelta = new Vector2(0, underline.sizeDelta.y);
        underlineImage.color = normalColor;
    }
}
