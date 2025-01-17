using UnityEngine;
using UnityEngine.EventSystems;

public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Vector3 hoverScale = new Vector3(1.2f, 1.2f, 1.2f);
    [SerializeField] private float transitionDuration = 0.2f;

    private Vector3 originalScale;
    private bool isHovered = false;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        StopAllCoroutines();
        StartCoroutine(ScaleOverTime(hoverScale));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        StopAllCoroutines();
        StartCoroutine(ScaleOverTime(originalScale));
    }

    private System.Collections.IEnumerator ScaleOverTime(Vector3 targetScale)
    {
        Vector3 currentScale = transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            transform.localScale = Vector3.Lerp(currentScale, targetScale, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }
}
