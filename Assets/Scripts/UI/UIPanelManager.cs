using UnityEngine;
using DG.Tweening;

public abstract class UIPanelManager : MonoBehaviour
{
    [SerializeField] public RectTransform panel;
    [SerializeField] public Vector2 hiddenPosition;
    [SerializeField] public Vector2 visiblePosition;
    [SerializeField] public float animationDuration = 0.5f;

    public virtual void ShowPanel()
    {
        panel.DOAnchorPos(visiblePosition, animationDuration).SetEase(Ease.OutQuad);
    }

    public virtual void HidePanel()
    {
        panel.DOAnchorPos(hiddenPosition, animationDuration).SetEase(Ease.InQuad);
    }
}

