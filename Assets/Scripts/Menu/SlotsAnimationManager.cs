using UnityEngine;
using DG.Tweening;

public class SlotsAnimationManager : MonoBehaviour
{
    [Header("Slot Settings")]
    [SerializeField] private RectTransform[] slots;
    [SerializeField] private Vector2 spawnPosition;
    [SerializeField] private Vector2[] targetPositions;
    [SerializeField] private float spawnInterval = 0.3f; 
    [SerializeField] private float moveDuration = 0.8f;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float titleMoveDuration = 0.4f;
    [SerializeField] private RectTransform gameTitle;

    private void Start()
    {
        // Ініціалізація слотів
        for (int i = 0; i < slots.Length; i++)
        {
            // Встановлюємо початкову позицію і прозорість
            slots[i].anchoredPosition = spawnPosition;
            var canvasGroup = slots[i].GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = slots[i].gameObject.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = 0; // Початкова прозорість
        }
    }

    public void AnimateSlots()
    {
        gameTitle.DOAnchorPosY(gameTitle.anchoredPosition.y + 100, titleMoveDuration).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            for (int i = 0; i < slots.Length; i++)
            {
                RectTransform slot = slots[i];
                Vector2 targetPosition = targetPositions[i];

                // Відкладена анімація для кожного слота
                DOVirtual.DelayedCall(i * spawnInterval, () =>
                {
                    var canvasGroup = slot.GetComponent<CanvasGroup>();

                    // Анімація прозорості
                    canvasGroup.DOFade(1, fadeDuration);

                    // Анімація переміщення в цільову позицію
                    slot.DOAnchorPos(targetPosition, moveDuration).SetEase(Ease.OutQuad);
                });
            }
        });
    }
    public void HideSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            RectTransform slot = slots[i];

            // Зникнення слотів із поверненням у початкову позицію
            DOVirtual.DelayedCall(i * spawnInterval, () =>
            {
                var canvasGroup = slot.GetComponent<CanvasGroup>();
                if (canvasGroup == null) canvasGroup = slot.gameObject.AddComponent<CanvasGroup>();

                canvasGroup.DOFade(0, fadeDuration);
                slot.DOAnchorPos(spawnPosition, moveDuration).SetEase(Ease.InQuad);
            });
        }
    }
}
