using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Pan Settings")]
    [SerializeField] private float basePanSpeed = 10f;
    [SerializeField] private Vector2 panLimit = new Vector2(50f, 50f);
    [SerializeField] private float shiftMultiplier = 2f;
    [SerializeField] private float inertiaDuration = 0.2f;

    [Header("Scroll Settings")]
    [SerializeField] private float scrollSpeed = 5f;
    [SerializeField] private float minHeight = 10f;
    [SerializeField] private float maxHeight = 50f;
    [SerializeField] private float scrollSmoothTime = 0.1f;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private float minRotationX = 60f;
    [SerializeField] private float maxRotationX = 90f;
    [SerializeField] private Vector3 initialRotation = new Vector3(80f, 0f, 0f);

    [Header("Initial Settings")]
    [SerializeField] private Vector3 initialPosition = new Vector3(0f, 20f, 0f);

    [Header("Collision Settings")]
    [SerializeField] private LayerMask terrainLayerMask;
    [SerializeField] private float collisionOffset = 1f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPosition;
    private float targetHeight;
    private float heightVelocity;
    private float rotationX;
    private float rotationY;

    public static bool IsInputBlocked { get; set; } = false;

    private void Start()
    {
        transform.position = initialPosition;
        transform.rotation = Quaternion.Euler(initialRotation);
        targetPosition = transform.position;
        targetHeight = transform.position.y;
        rotationX = initialRotation.x;
        rotationY = initialRotation.y;
    }

    private void Update()
    {
        if (IsInputBlocked) return;

        HandleScroll();
        HandlePan();
        HandleRotation();
        ClampPosition();
        PreventCollision();
    }

    private void HandleScroll()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        targetHeight -= scroll * scrollSpeed;
        targetHeight = Mathf.Clamp(targetHeight, minHeight, maxHeight);

        if (Physics.Raycast(new Vector3(transform.position.x, targetHeight + 100f, transform.position.z), Vector3.down, out RaycastHit hit, Mathf.Infinity, terrainLayerMask))
        {
            float terrainHeight = hit.point.y + collisionOffset;
            if (targetHeight < terrainHeight)
            {
                targetHeight = terrainHeight;
            }
        }

        targetPosition.y = Mathf.SmoothDamp(transform.position.y, targetHeight, ref heightVelocity, scrollSmoothTime);
    }


    private void HandlePan()
    {
        float speedMultiplier = Input.GetKey(KeyCode.LeftShift) ? shiftMultiplier : 1f;
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) movement += forward * basePanSpeed * speedMultiplier * Time.deltaTime;
        if (Input.GetKey(KeyCode.S)) movement -= forward * basePanSpeed * speedMultiplier * Time.deltaTime;
        if (Input.GetKey(KeyCode.A)) movement -= right * basePanSpeed * speedMultiplier * Time.deltaTime;
        if (Input.GetKey(KeyCode.D)) movement += right * basePanSpeed * speedMultiplier * Time.deltaTime;

        if (movement != Vector3.zero)
        {
            targetPosition += movement;
        }

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, inertiaDuration);
    }

    private void HandleRotation()
    {
        if (Input.GetMouseButton(1))
        {
            rotationX -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            rotationY += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;

            rotationX = Mathf.Clamp(rotationX, minRotationX, maxRotationX);

            transform.rotation = Quaternion.Euler(rotationX, rotationY, 0f);
        }
    }

    private void ClampPosition()
    {
        targetPosition.x = Mathf.Clamp(targetPosition.x, -panLimit.x, panLimit.x);
        targetPosition.z = Mathf.Clamp(targetPosition.z, -panLimit.y, panLimit.y);
    }

    private void PreventCollision()
    {
        float sphereRadius = collisionOffset;
        Vector3 movementDirection = (targetPosition - transform.position).normalized;
        float movementDistance = Vector3.Distance(transform.position, targetPosition);

        if (Physics.SphereCast(transform.position, sphereRadius, movementDirection, out RaycastHit hit, movementDistance, terrainLayerMask))
        {
            targetPosition = hit.point - movementDirection * sphereRadius;
        }

        if (Physics.Raycast(targetPosition + Vector3.up * 100f, Vector3.down, out RaycastHit verticalHit, Mathf.Infinity, terrainLayerMask))
        {
            float terrainHeight = verticalHit.point.y;
            if (targetPosition.y < terrainHeight + collisionOffset)
            {
                targetPosition.y = terrainHeight + collisionOffset;
            }
        }

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, inertiaDuration);
    }
}