using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private static CameraShake _instance;

    private Transform cameraTransform;
    private Vector3 originalPosition;

    private float shakeDuration;
    private float shakeIntensity;
    private float shakeTimer;

    [Header("Shake Settings")]
    [SerializeField] private float defaultAmplitudeMultiplier = 1f;
    [SerializeField] private float shakeFrequency = 25f;

    public static CameraShake Instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = new GameObject("CameraShake");
                _instance = obj.AddComponent<CameraShake>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

            cameraTransform = Camera.main?.transform;
            if (cameraTransform == null)
            {
                Debug.LogError("No Camera found in the scene.");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Shake(float intensity, float duration, float amplitudeMultiplier = -1f)
    {
        if (cameraTransform == null) return;

        shakeIntensity = intensity;
        shakeDuration = duration;
        shakeTimer = duration;

        if (amplitudeMultiplier > 0)
        {
            defaultAmplitudeMultiplier = amplitudeMultiplier;
        }
    }

    public void ShakeWithDistance(Vector3 sourcePosition, float maxIntensity, float maxDistance, float amplitudeMultiplier = -1f)
    {
        if (cameraTransform == null) return;

        float distance = Vector3.Distance(cameraTransform.position, sourcePosition);
        float intensity = Mathf.Clamp01(1 - (distance / maxDistance)) * maxIntensity;

        if (intensity > 0)
        {
            Shake(intensity, 0.3f, amplitudeMultiplier);
        }
    }

    private void LateUpdate()
    {
        if (cameraTransform == null) return;

        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;

            float currentIntensity = Mathf.Lerp(0, shakeIntensity, shakeTimer / shakeDuration);
            Vector3 randomOffset = new Vector3(
                Mathf.Sin(Time.time * shakeFrequency) * defaultAmplitudeMultiplier,
                Mathf.Cos(Time.time * shakeFrequency) * defaultAmplitudeMultiplier,
                0
            ) * currentIntensity;

            cameraTransform.position += randomOffset;
        }
    }
}
