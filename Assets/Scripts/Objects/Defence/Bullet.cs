using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float BulletSpeed = 200f;
    [SerializeField] private float lifetime = 5f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifetime);
    }

    private void Start()
    {
        rb.velocity = transform.up * BulletSpeed;
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