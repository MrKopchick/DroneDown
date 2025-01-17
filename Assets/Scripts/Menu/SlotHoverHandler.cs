using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SlotHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float hoverScale = 1.2f;
    [SerializeField] private float hoverDuration = 0.2f; 

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(hoverScale, hoverDuration).SetEase(Ease.OutQuad);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1f, hoverDuration).SetEase(Ease.OutQuad);
    }
}
