using UnityEngine;
using DG.Tweening;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Frames")]
    [SerializeField] private GameObject firstFrame; // Головна сторінка
    [SerializeField] private GameObject secondFrame; // Сторінка слотів збережень
    [SerializeField] private SlotsAnimationManager slotsAnimationManager; // Менеджер слотів

    [Header("Animation Settings")]
    [SerializeField] private float fadeDuration = 0.5f; // Тривалість затухання
    [SerializeField] private float moveDuration = 0.5f; // Тривалість переміщення панелей

    private bool isInSecondFrame = false; // Визначає, чи користувач у другому кадрі

    private void Update()
    {
        // Обробка кнопки Escape
        if (Input.GetKeyDown(KeyCode.Escape) && isInSecondFrame)
        {
            BackToMainMenu();
        }
    }

    public void OnStartButtonClicked()
    {
        // Переходить до другого кадру з анімаціями
        isInSecondFrame = true;

        HidePanel(firstFrame, () =>
        {
            secondFrame.SetActive(true);
            ShowPanel(secondFrame);
            slotsAnimationManager.AnimateSlots(); // Анімація появи слотів
        });
    }

    public void BackToMainMenu()
    {
        if (!isInSecondFrame) return;

        isInSecondFrame = false;

        // Викликаємо анімацію зникнення слотів
        slotsAnimationManager.HideSlots();

        // Приховуємо другий кадр і повертаємо головний
        HidePanel(secondFrame, () =>
        {
            firstFrame.SetActive(true);
            ShowPanel(firstFrame);
        });
    }

    private void HidePanel(GameObject panel, TweenCallback onComplete = null)
    {
        // Приховуємо панель через CanvasGroup із fade ефектом
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = panel.AddComponent<CanvasGroup>();

        canvasGroup.DOFade(0, fadeDuration).OnComplete(() =>
        {
            panel.SetActive(false);
            canvasGroup.alpha = 1; // Відновлюємо прозорість
            onComplete?.Invoke();
        });
    }

    private void ShowPanel(GameObject panel)
    {
        // Показуємо панель через CanvasGroup із fade ефектом
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = panel.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, fadeDuration);
    }
}
