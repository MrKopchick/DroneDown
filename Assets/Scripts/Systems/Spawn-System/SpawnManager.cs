using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private LayerMask placementLayer;
    [SerializeField] private float maxSlopeAngle = 30f;
    [SerializeField] private float minDistanceBetweenObjects = 5f;
    [SerializeField] private float maxHeightDifference = 2f;

    private SpawnButton activeButton;
    private SpawnableObject currentSpawnableObject;
    private GameObject previewObject;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    public void SetSpawnableObject(SpawnableObject spawnableObject, SpawnButton button = null)
    {
        if (activeButton != null && activeButton != button)
        {
            activeButton.ResetButtonState();
        }

        currentSpawnableObject = spawnableObject;
        activeButton = button;

        if (previewObject != null)
        {
            Destroy(previewObject);
        }

        if (currentSpawnableObject != null && currentSpawnableObject.previewPrefab != null)
        {
            previewObject = Instantiate(currentSpawnableObject.previewPrefab);
        }
    }

    private void Update()
    {
        if (currentSpawnableObject == null || mainCamera == null) return;

        if (EventSystem.current.IsPointerOverGameObject()) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, placementLayer))
        {
            bool isValid = CanPlaceObject(hit.point);
            UpdatePreview(hit.point, isValid);

            if (Input.GetMouseButtonDown(0) && isValid)
            {
                PlaceObject(hit.point);
            }
        }
        else
        {
            RemovePreview();
        }
    }

    private bool CanPlaceObject(Vector3 position)
    {
        if (Physics.Raycast(position + Vector3.up * 10f, Vector3.down, out RaycastHit hit, 20f, placementLayer))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            if (slopeAngle > maxSlopeAngle) return false;

            Collider[] colliders = Physics.OverlapSphere(position, minDistanceBetweenObjects);
            foreach (var collider in colliders)
            {
                if (collider.gameObject.CompareTag("SpawnedObject"))
                {
                    return false;
                }
            }

            if (Mathf.Abs(hit.point.y - position.y) > maxHeightDifference)
            {
                return false;
            }

            return true;
        }
        return false;
    }

    private void UpdatePreview(Vector3 position, bool isValid)
    {
        if (previewObject == null) return;

        previewObject.transform.position = position;

        Renderer renderer = previewObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = isValid ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.5f);
        }
    }

    private void RemovePreview()
    {
        if (previewObject != null)
        {
            Destroy(previewObject);
        }
    }

    private void PlaceObject(Vector3 position)
    {
        if (currentSpawnableObject == null || currentSpawnableObject.prefab == null) return;

        GameObject newObject = Instantiate(currentSpawnableObject.prefab, position, Quaternion.identity);
        newObject.tag = "SpawnedObject";

        var construction = newObject.GetComponent<ConstructionProcess>();
        if (construction != null)
        {
            ConstructionQueue.Instance.AddToQueue(construction);
        }

        RemovePreview();

        if (activeButton != null)
        {
            activeButton.ResetButtonState();
            activeButton = null;
        }

        SetSpawnableObject(null);
    }
}
