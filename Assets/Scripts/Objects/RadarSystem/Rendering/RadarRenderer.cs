using System.Collections.Generic;
using RadarSystem.Data;
using UnityEngine;

namespace RadarSystem{
    
    namespace Rendering
    {
        public class RadarRenderer : MonoBehaviour
        {
            private RadarSettings _settings;
            private Transform _radarDish;

            public void Initialize(RadarSettings settings, Transform radarDish)
            {
                _settings = settings;
                _radarDish = radarDish;
            }

            public void UpdateDisplay(ProcessedRadarData data)
            {
                ClearDisplay();

                foreach (var target in data.ValidTargets)
                {
                    DrawTarget(target);
                }
            }

            private void ClearDisplay()
            {
                // 
            }

            private void DrawTarget(RadarTarget target)
            {
                Vector3 screenPosition = ConvertToRadarCoordinates(target.Position);
                Debug.DrawLine(screenPosition, screenPosition + Vector3.up * 0.1f, Color.red, 2f);
            }

            private Vector3 ConvertToRadarCoordinates(Vector3 worldPosition)
            {
                Vector3 direction = worldPosition - _radarDish.position;
                float distance = direction.magnitude;
                float angle = Vector3.Angle(_radarDish.forward, direction);

                float scaledX = (distance / _settings.Range) * Mathf.Cos(angle * Mathf.Deg2Rad);
                float scaledY = (distance / _settings.Range) * Mathf.Sin(angle * Mathf.Deg2Rad);

                return new Vector3(scaledX, 0, scaledY);
            }
        }
    }
}