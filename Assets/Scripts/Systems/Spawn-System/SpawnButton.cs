using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Economy;
using Game.Spawning.Data;
using Game.Spawning.Controllers;

namespace Game.UI.Buttons
{
    public sealed class SpawnButton : MonoBehaviour
    {
        [SerializeField] private SpawnableObject spawnableObject;
        [SerializeField] private SpawnController spawnController;
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
            button.onClick.AddListener(OnButtonClick);
            UpdateButtonState();
        }

        private void OnButtonClick()
        {
            if (isSpawning)
            {
                spawnController.CancelSpawning();
                isSpawning = false;
                NotificationManager.Instance.ShowMessage("Spawn canceled.", "yellow");
            }
            else if (!EconomyManager.Instance.Spend(spawnableObject.price))
            {
                AnimateButtonError();
                NotificationManager.Instance.ShowMessage("Not enough money!", "red");
                return;
            }
            else
            {
                spawnController.SetActiveSpawnable(spawnableObject);
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

            buttonImage.color = errorColor;
            Invoke(nameof(ResetButtonColor), 0.2f);
        }

        private void ResetButtonColor()
        {
            if (buttonImage != null)
            {
                buttonImage.color = spawnColor;
            }
        }

        public void ResetState()
        {
            isSpawning = false;
            UpdateButtonState();
        }
    }
}