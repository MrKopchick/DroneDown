using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Hover Effect Settings")]
    [SerializeField] private float scaleMultiplier = 1.1f; // Коефіцієнт збільшення
    [SerializeField] private float brightnessMultiplier = 1.2f; // Коефіцієнт збільшення яскравості
    [SerializeField] private float hoverDuration = 0.2f; // Тривалість ефекту

    private Vector3 originalScale; // Початковий масштаб кнопки
    private Color originalColor; // Початковий колір кнопки
    private Image buttonImage; // Зображення кнопки

    private void Awake()
    {
        buttonImage = GetComponent<Image>();

        if (buttonImage == null)
        {
            Debug.LogError($"Button {gameObject.name} не має компонента Image!");
            return;
        }

        originalScale = transform.localScale;
        originalColor = buttonImage.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Ефект збільшення масштабу
        transform.DOScale(originalScale * scaleMultiplier, hoverDuration).SetEase(Ease.OutQuad);

        // Ефект збільшення яскравості
        buttonImage.DOColor(GetBrightenedColor(originalColor, brightnessMultiplier), hoverDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Повернення до початкового масштабу
        transform.DOScale(originalScale, hoverDuration).SetEase(Ease.OutQuad);

        // Повернення до початкового кольору
        buttonImage.DOColor(originalColor, hoverDuration);
    }

    // Обчислення яскравішого кольору
    private Color GetBrightenedColor(Color color, float multiplier)
    {
        float r = Mathf.Clamp01(color.r * multiplier);
        float g = Mathf.Clamp01(color.g * multiplier);
        float b = Mathf.Clamp01(color.b * multiplier);
        return new Color(r, g, b, color.a);
    }
}
