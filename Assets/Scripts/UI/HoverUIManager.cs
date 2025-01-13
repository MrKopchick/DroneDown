using UnityEngine;
using  TMPro;
using DG.Tweening;
public class HoverUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text hoverText;
    private Camera mainCamera;
    private Vector3 offset = new Vector3(0, 30f, 0);
    private bool isHovering = false;

    public static HoverUIManager Instance;

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
    }

    private void Start()
    {
        mainCamera = Camera.main;
        hoverText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isHovering)
        {
            UpdateHoverPosition();
        }
    }

    public void ShowHoverText(string text, Vector3 worldPosition)
    {
        hoverText.text = text;
        
        hoverText.gameObject.SetActive(true);
        hoverText.color = new Color(hoverText.color.r, hoverText.color.g, hoverText.color.b, 0);
        hoverText.DOFade(1f, 0.3f).SetEase(Ease.OutQuad);

        isHovering = true;
        UpdateHoverPosition();
    }

    public void HideHoverText()
    {
        hoverText.DOKill();
        hoverText.color = new Color(hoverText.color.r, hoverText.color.g, hoverText.color.b, 0);
        hoverText.gameObject.SetActive(false);

        isHovering = false;
    }

    private void UpdateHoverPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        hoverText.transform.position = mousePosition + offset;
    }
}