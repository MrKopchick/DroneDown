using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SpawnButton : MonoBehaviour
{
    [SerializeField] private SpawnableObject spawnableObject;
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private Color spawnColor = Color.green;
    [SerializeField] private Color cancelColor = Color.red;
    [SerializeField] private Color errorColor = Color.red;

    private Button button;
    private bool isSpawning = false;
    private Image buttonImage;

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();

        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }

        UpdateButtonState();
    }

    private void OnButtonClick()
    {
        if (isSpawning)
        {
            spawnManager.SetSpawnableObject(null);
            isSpawning = false;
            NotificationManager.Instance.ShowMessage("Spawn canceled.", "yellow");
        }
        else if (!EconomyManager.Instance.SpendMoney(spawnableObject.price))
        {
            AnimateButtonError();
            NotificationManager.Instance.ShowMessage("Not enough money!", "red");
            return;
        }
        else
        {
            spawnManager.SetSpawnableObject(spawnableObject, this);
            isSpawning = true;
            NotificationManager.Instance.ShowMessage($"Started spawning: {spawnableObject.objectName}.", "green");
        }

        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        if (buttonImage != null)
        {
            buttonImage.color = isSpawning ? cancelColor : spawnColor;
        }

        if (button != null)
        {
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = isSpawning ? "Cancel" : "Build";
            }
        }
    }

    private void AnimateButtonError()
    {
        if (buttonImage == null) return;
        
        Color originalColor = buttonImage.color;
        CameraShake.Instance.Shake(0.1f, 0.3f);
            
        DOTween.Sequence()
            .Append(buttonImage.DOColor(errorColor, 0.2f))
            .Append(buttonImage.DOColor(originalColor, 0.2f));
    }

    public void ResetButtonState()
    {
        isSpawning = false;
        CameraShake.Instance.Shake(0.015f, 0.15f);
        UpdateButtonState();
    }

    public void SetInteractable(bool state)
    {
        if (button != null)
        {
            button.interactable = state;

            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.color = state ? Color.white : Color.gray;
            }
        }
    }
}
