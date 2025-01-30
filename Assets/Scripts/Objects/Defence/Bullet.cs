using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float speed = 20f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
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