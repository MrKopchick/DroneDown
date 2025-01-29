using System.Collections.Generic;
using UnityEngine;

public class RadarObject : MonoBehaviour
{
    [Header("Radar Settings")]
    [SerializeField] private float radarRange = 10f;
    [SerializeField] private float radarRefreshRate = 1f;
    [SerializeField] private float radarRotationSpeed = 10f;
    [SerializeField] private float radarRotationAngle = 90f;
    [SerializeField] private float heightRange = 50f;
    [SerializeField] private List<string> targetTags;

    [Header("Radar Components")]
    [SerializeField] private Transform radarDish;

    private float lastUpdateTime;

    void Update()
    {
        DrawRadarRange(radarDish.forward);
        RotateRadar();
        lastUpdateTime += Time.deltaTime;

        if (lastUpdateTime >= radarRefreshRate)
        {
            ScanForTargets();
            lastUpdateTime = 0;
        }
    }

    private void RotateRadar()
    {
        radarDish.Rotate(Vector3.up, radarRotationSpeed * Time.deltaTime);
    }

    private void ScanForTargets()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radarRange);
        Vector3 forwardDirection = radarDish.forward;

        DrawRadarRange(forwardDirection);

        foreach (var hit in hitColliders)
        {
            if (targetTags.Contains(hit.tag))
            {
                Vector3 directionToTarget = (hit.transform.position - transform.position).normalized;
                float angle = Vector3.Angle(forwardDirection, directionToTarget);

                if (angle <= radarRotationAngle / 2 &&
                    Mathf.Abs(hit.transform.position.y - transform.position.y) <= heightRange)
                {
                    Debug.Log($"Target found at position: X: {hit.transform.position.x}, Z: {hit.transform.position.z}");
                    Debug.DrawLine(transform.position, hit.transform.position, Color.red, 2f);
                }
            }
        }
    }

    private void DrawRadarRange(Vector3 forwardDirection)
    {
        float halfAngle = radarRotationAngle / 2;
        Vector3 leftBoundary = Quaternion.Euler(0, -halfAngle, 0) * forwardDirection;
        Vector3 rightBoundary = Quaternion.Euler(0, halfAngle, 0) * forwardDirection;

        Vector3 basePosition = transform.position;

        Vector3 upperBoundary = basePosition + Vector3.up * heightRange;
        Debug.DrawLine(basePosition, upperBoundary + leftBoundary * radarRange, Color.blue);
        Debug.DrawLine(basePosition, upperBoundary + rightBoundary * radarRange, Color.blue);

        Vector3 lowerBoundary = basePosition - Vector3.up * heightRange;
        Debug.DrawLine(basePosition, lowerBoundary + leftBoundary * radarRange, Color.blue);
        Debug.DrawLine(basePosition, lowerBoundary + rightBoundary * radarRange, Color.blue);

        Debug.DrawLine(basePosition, basePosition + leftBoundary * radarRange, Color.green);
        Debug.DrawLine(basePosition, basePosition + rightBoundary * radarRange, Color.green);
    }
}