using UnityEngine;

public class Mine : MonoBehaviour
{
    [Header("Mining Settings")]
    [SerializeField] private float miningSpeed = 5f;

    private bool isActive = true;

    private void Start()
    {
        InvokeRepeating(nameof(MineResources), 0f, 1f);
    }

    private void MineResources()
    {
        if (!isActive) return;

        GameManager.Instance.AddResources(Mathf.FloorToInt(miningSpeed));
    }

    public void ToggleActivity(bool active)
    {
        isActive = active;
    }

    public float GetMiningSpeed()
    {
        return miningSpeed;
    }

    public void SetMiningSpeed(float newSpeed)
    {
        miningSpeed = Mathf.Max(0, newSpeed);
    }

    public bool IsActive
    {
        get => isActive;
    }
}