using System.Collections.Generic;
using UnityEngine;
using RadarSystem.Data;

namespace RadarSystem{
    namespace Core
    {
        public class RadarScanner : MonoBehaviour
        {
            public delegate void TargetsDetectedHandler(RadarDetectionResult result);
            public event TargetsDetectedHandler OnTargetsDetected;

            private RadarSettings _settings;
            private Transform _radarDish;
            private float _lastUpdateTime;

            public void Initialize(RadarSettings settings, Transform radarDish)
            {
                _settings = settings;
                _radarDish = radarDish;
            }

            private void Update()
            {
                _lastUpdateTime += Time.deltaTime;

                if (_lastUpdateTime >= _settings.RefreshRate)
                {
                    ScanForTargets();
                    _lastUpdateTime = 0;
                }

                RotateRadar();
            }

            private void RotateRadar()
            {
                _radarDish.Rotate(Vector3.up, _settings.RotationSpeed * Time.deltaTime);
            }

            private void ScanForTargets()
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, _settings.Range);
                List<RadarTarget> detectedTargets = new List<RadarTarget>();

                foreach (var hit in hitColliders)
                {
                    if (_settings.TargetTags.Contains(hit.tag))
                    {
                        Vector3 directionToTarget = (hit.transform.position - transform.position).normalized;
                        float angle = Vector3.Angle(_radarDish.forward, directionToTarget);

                        if (angle <= _settings.RotationAngle / 2 &&
                            Mathf.Abs(hit.transform.position.y - transform.position.y) <= _settings.HeightRange)
                        {
                            detectedTargets.Add(new RadarTarget
                            {
                                Position = hit.transform.position,
                                Tag = hit.tag
                            });
                        }
                    }
                }

                if (detectedTargets.Count > 0)
                {
                    OnTargetsDetected?.Invoke(new RadarDetectionResult
                    {
                        RadarPosition = transform.position,
                        DetectedTargets = detectedTargets
                    });
                }
            }
        }
    }
}