using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    private DateTime inGameTime;
    private float realTimeSecondsPerStep = 0.5f;
    private float timeAccumulator = 0f;

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

        inGameTime = new DateTime(2032, 9, 1, 11, 0, 0);
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