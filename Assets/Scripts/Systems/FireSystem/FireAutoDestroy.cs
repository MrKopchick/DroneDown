using UnityEngine;

public class FireAutoDestroy : MonoBehaviour
{
    [SerializeField] private float lifetime = 180f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
