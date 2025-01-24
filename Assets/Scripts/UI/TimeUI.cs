using System;
using System.Globalization;
using UnityEngine;
using TMPro;

public class TimeUI : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;

    private DateTime lastDisplayedTime;

    private void Start()
    {
        UpdateTimeUI();
    }

    private void Update()
    {
        if (TimeManager.Instance == null) return;

        DateTime currentTime = TimeManager.Instance.CurrentTime;
        
        if (currentTime != lastDisplayedTime)
        {
            UpdateTimeUI();
            lastDisplayedTime = currentTime;
        }
    }

    private void UpdateTimeUI()
    {
        if (TimeManager.Instance == null || timeText == null) return;

        DateTime currentTime = TimeManager.Instance.CurrentTime;
        
        timeText.text = currentTime.ToString("yyyy MMMM dd HH:mm", CultureInfo.InvariantCulture);
    }
    
    
}