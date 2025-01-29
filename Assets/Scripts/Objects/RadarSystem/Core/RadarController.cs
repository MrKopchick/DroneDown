using System.Collections.Generic;
using UnityEngine;
using RadarSystem.Data;
using RadarSystem.Rendering;

namespace RadarSystem
{
    namespace Core
    {
        [RequireComponent(typeof(RadarScanner), typeof(RadarRenderer))]
        public class RadarController : MonoBehaviour
        {
            [SerializeField] private RadarSettings _settings;
            [SerializeField] private Transform _radarDish;
            
            private RadarScanner _scanner;
            private RadarRenderer _renderer;
            private IRadarDataHandler _dataHandler;

            private void Awake()
            {
                _scanner = GetComponent<RadarScanner>();
                _renderer = GetComponent<RadarRenderer>();
                _dataHandler = new RadarDataProcessor();

                InitializeComponents();
            }

            private void InitializeComponents()
            {
                _scanner.Initialize(_settings, _radarDish);
                _renderer.Initialize(_settings, _radarDish);
                
                _scanner.OnTargetsDetected += HandleDetectionResults;
            }

            private void HandleDetectionResults(RadarDetectionResult result)
            {
                var processedData = _dataHandler.ProcessDetectionResult(result);
                _renderer.UpdateDisplay(processedData);
            }

            private void OnDestroy()
            {
                _scanner.OnTargetsDetected -= HandleDetectionResults;
            }
        }
    }
}