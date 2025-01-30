using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifetime = 5f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifetime);
    }

    private void Start()
    {
        rb.velocity = transform.forward * speed;
    }

    public void Initialize(float bulletSpeed)
    {
        speed = bulletSpeed;
        if (rb != null)
        {
            rb.velocity = transform.forward * speed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Shahed"))
        {
            ShahedHealth shahedHealth = other.GetComponent<ShahedHealth>();
            if (shahedHealth != null)
            {
                shahedHealth.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}