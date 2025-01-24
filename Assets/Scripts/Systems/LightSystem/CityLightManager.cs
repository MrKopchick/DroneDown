using UnityEngine;
using System.Collections.Generic;

public class CityLightManager : ToggleLightByTime
{
    [Header("City Light Settings")]
    [SerializeField] private string lightTag = "CityLight";
    [SerializeField, Range(1f, 10f)] private float transitionDuration = 5f;

    private List<Light> cityLights = new List<Light>();
    private float transitionTimer = 0f;
    private bool lightsOn = false;

    private void Start()
    {
        FindCityLights();
    }

    private void FindCityLights()
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(lightTag);
        foreach (GameObject obj in taggedObjects)
        {
            Light light = obj.GetComponent<Light>();
            if (light != null)
            {
                cityLights.Add(light);
            }
        }
    }

    protected override void Update()
    {
        base.Update();

        if (TimeManager.Instance != null)
        {
            UpdateCityLights();
        }
    }

    private void UpdateCityLights()
    {
        int currentHour = TimeManager.Instance.CurrentTime.Hour;

        if (currentHour >= 20 || currentHour < 12)
        {
            if (!lightsOn)
            {
                lightsOn = true;
                transitionTimer = 0f;
            }
        }
        else
        {
            if (lightsOn)
            {
                lightsOn = false;
                transitionTimer = 0f;
            }
        }

        SmoothTransition(lightsOn);
    }

    private void SmoothTransition(bool turnOn)
    {
        transitionTimer += Time.deltaTime;

        float t = Mathf.Clamp01(transitionTimer / transitionDuration);
        float targetIntensity = turnOn ? maxLightIntensity / 255f : minLightIntensity / 255f;

        foreach (Light light in cityLights)
        {
            if (light != null)
            {
                light.intensity = Mathf.Lerp(light.intensity, targetIntensity, t);
            }
        }
    }
}
