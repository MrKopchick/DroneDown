using Game.Spawning.Data;
using Game.Spawning.Preview;
using UnityEngine;
using Game.Spawning.Placement;
using Game.Economy;

namespace Game.Spawning.Controllers
{
    public sealed class SpawnController : MonoBehaviour, ISpawnController
    {
        [SerializeField] private LayerMask placementMask;
        
        private PreviewManager previewManager;
        private PlacementValidator placementValidator;
        private ObjectPlacer objectPlacer;
        private SpawnInputHandler inputHandler;
        
        private SpawnableObject currentSpawnable;

        private void Awake()
        {
            previewManager = new PreviewManager();
            placementValidator = new PlacementValidator(placementMask);
            objectPlacer = new ObjectPlacer();
            inputHandler = new SpawnInputHandler(this);
        }

        public void SetActiveSpawnable(SpawnableObject spawnable)
        {
            currentSpawnable = spawnable;
            previewManager.CreatePreview(spawnable.previewPrefab);
        }

        public void CancelSpawning()
        {
            currentSpawnable = null;
            previewManager.DestroyPreview();
        }

        private void Update()
        {
            if (!currentSpawnable) return;
            
            inputHandler.HandleInput();
            UpdatePreview();
        }

        private void UpdatePreview()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit, Mathf.Infinity, placementMask)) return;

            var isValid = placementValidator.IsPositionValid(hit.point, currentSpawnable);
            previewManager.UpdatePreview(hit.point, isValid);
            
            if (Input.GetMouseButtonDown(0) && isValid)
                PlaceObject(hit.point);
        }

        private void PlaceObject(Vector3 position)
        {
            if (!EconomyManager.Instance.Spend(currentSpawnable.price)) return;
            
            objectPlacer.PlaceObject(currentSpawnable.prefab, position, previewManager.CurrentRotation);
            CancelSpawning();
        }

        public void RotatePreview(float degrees) => previewManager.Rotate(degrees);
    }
}