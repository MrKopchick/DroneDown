using UnityEngine;

public class ToggleLightByTime : MonoBehaviour
{
    [Header("Directional Light Reference")]
    [SerializeField] private Light directionalLight;

    [Header("Lighting Settings")]
    [SerializeField, Range(70f, 255f)] private float minLightIntensity = 70f;
    [SerializeField, Range(70f, 255f)] private float maxLightIntensity = 255f;

    [Header("Time Settings")]
    [SerializeField] private int sunriseHour = 2;
    [SerializeField] private int sunsetHour = 15;

    private void Update()
    {
        if (directionalLight == null || TimeManager.Instance == null) return;

        UpdateLightBasedOnTime();
    }

    private void UpdateLightBasedOnTime()
    {
        int currentHour = TimeManager.Instance.CurrentTime.Hour;
        float currentMinute = TimeManager.Instance.CurrentTime.Minute;

        float normalizedTime = CalculateNormalizedTime(currentHour, currentMinute);
        float intensity = Mathf.Lerp(minLightIntensity / 255f, maxLightIntensity / 255f, normalizedTime);
        directionalLight.intensity = intensity;

        directionalLight.color = Color.Lerp(
            new Color(minLightIntensity / 255f, minLightIntensity / 255f, minLightIntensity / 255f),
            new Color(maxLightIntensity / 255f, maxLightIntensity / 255f, maxLightIntensity / 255f),
            normalizedTime
        );
    }

    private float CalculateNormalizedTime(int hour, float minute)
    {
        if (hour >= sunriseHour && hour <= sunsetHour)
        {
            float totalMinutes = ((hour - sunriseHour) * 60f) + minute;
            float dayDuration = (sunsetHour - sunriseHour) * 60f;
            return Mathf.Clamp01(totalMinutes / dayDuration);
        }
        else
        {
            float totalMinutes;
            if (hour < sunriseHour)
            {
                totalMinutes = ((hour + 24 - sunsetHour) * 60f) + minute;
            }
            else
            {
                totalMinutes = ((hour - sunsetHour) * 60f) + minute;
            }

            float nightDuration = ((24 - sunsetHour) + sunriseHour) * 60f;
            return Mathf.Clamp01(1f - (totalMinutes / nightDuration));
        }
    }
}
