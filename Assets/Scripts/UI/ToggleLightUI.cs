using UnityEngine;
using UnityEngine.UI;

public class ToggleLightUI : MonoBehaviour
{
    [Header("Directional Light Reference")]
    [SerializeField] private Light directionalLight;

    [Header("UI Button")]
    [SerializeField] private Button toggleButton;

    [Header("Transition Settings")]
    [SerializeField] private float transitionDuration = 1.0f;

    private bool isDarkMode = false;

    private void Start()
    {
        if (toggleButton != null)
        {
            toggleButton.onClick.AddListener(ToggleLight);
        }

        if (directionalLight == null)
        {
            Debug.LogError("Directional Light is not assigned!");
        }
    }

    private void ToggleLight()
    {
        if (directionalLight == null) return;

        Color startColor = directionalLight.color;
        Color targetColor = isDarkMode ? Color.white : new Color(70f / 255f, 70f / 255f, 70f / 255f);

        isDarkMode = !isDarkMode;
        StopAllCoroutines();
        StartCoroutine(LerpLightColor(startColor, targetColor));
    }

    private System.Collections.IEnumerator LerpLightColor(Color startColor, Color targetColor)
    {
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            directionalLight.color = Color.Lerp(startColor, targetColor, elapsedTime / transitionDuration);
            yield return null;
        }

        directionalLight.color = targetColor;
    }
}