using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    private IDCardBase idCard;
    private CityIdCard cityCard;

    private void Start()
    {
        idCard = GetComponent<IDCardBase>();
        cityCard = GetComponent<CityIdCard>();
    }

    private void OnMouseEnter()
    {
        if (HoverUIManager.Instance != null)
        {
            if (idCard != null)
            {
                HoverUIManager.Instance.ShowHoverText(idCard.ObjectName, transform.position);
            }
            else if (cityCard != null)
            {
                HoverUIManager.Instance.ShowHoverText(cityCard.GetCityCardContent().Split('\n')[0], transform.position);
            }
        }
    }

    private void OnMouseExit()
    {
        if (HoverUIManager.Instance != null)
        {
            HoverUIManager.Instance.HideHoverText();
        }
    }

    private void OnMouseDown()
    {

        if (idCard != null)
        {
            IDCardManager.Instance.ShowIDCard(idCard);
        }
        else if (cityCard != null)
        {
            IDCardManager.Instance.ShowCityCard(cityCard);
        }
        else
        {
            Debug.LogError($"No valid ID card found for {gameObject.name}!");
        }
    }
}