using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    private DateTime inGameTime;
    private float realTimeSecondsPerStep = 0.5f;
    private float timeAccumulator = 0f;

    [Header("Advance Settings")]
    [SerializeField] private int startHour = 8;

    public DateTime CurrentTime => inGameTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        inGameTime = new DateTime(2032, 9, 1, startHour, 0, 0);
    }

    private void Update()
    {
        timeAccumulator += Time.deltaTime;

        if (timeAccumulator >= realTimeSecondsPerStep)
        {
            int minutesToAdd = Mathf.FloorToInt(timeAccumulator / realTimeSecondsPerStep);
            inGameTime = inGameTime.AddMinutes(minutesToAdd);
            timeAccumulator -= minutesToAdd * realTimeSecondsPerStep;
        }
    }
}