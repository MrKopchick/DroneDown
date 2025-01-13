using UnityEngine;
using System;

public class Missile : MonoBehaviour
{
    public Transform target;
    public float initialSpeed = 10f;
    public float maxSpeed = 40f;
    public float acceleration = 5f;
    public float rotationSpeed = 60f;
    public float collisionRadius = 1f;
    public float maxLifetime = 10f;
    public ParticleSystem trailEffect;
    public Transform modelTransform;

    private float currentSpeed;
    private bool isExploded = false;
    
    public event Action<Missile> OnMissed;
    public event Action<Missile> OnMissileDestroyed;

    private GameObject trailInstance;

    void Start()
    {
        if (trailEffect != null)
        {
            trailInstance = Instantiate(trailEffect.gameObject, transform.position, transform.rotation);
            trailInstance.transform.parent = null;
            trailEffect = trailInstance.GetComponent<ParticleSystem>();
            trailEffect.Play();
        }

        currentSpeed = initialSpeed;

        Invoke(nameof(DestroyMissile), maxLifetime);
        Destroy(gameObject, maxLifetime + 10f);
    }

    void Update()
    {
        if (isExploded || target == null)
        {
            HandleMiss();
            return;
        }
        CameraShake.Instance.ShakeWithDistance(gameObject.transform.position, 0.4f, 40f, 0.05f);
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        
        transform.position += transform.forward * currentSpeed * Time.deltaTime;
        currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);
        
        if (trailInstance != null)
        {
            trailInstance.transform.position = transform.position;
        }
        
        if (Vector3.Distance(transform.position, target.position) <= collisionRadius)
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (isExploded) return;
        isExploded = true;
        
        if (target != null)
        {
            //Debug.Log($"Target destroyed: {target.name}");
            Destroy(target.gameObject);
        }

        DestroyMissile();
    }

    private void HandleMiss()
    {
        if (!isExploded)
        {
            //Debug.Log("Missile missed the target.");
            OnMissed?.Invoke(this);
        }

        DestroyMissile();
    }

    private void DestroyMissile()
    {
        OnMissileDestroyed?.Invoke(this);

        if (trailEffect != null && trailEffect.isPlaying)
        {
            trailEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        DestroyModel();
        Destroy(gameObject, 2f);
    }

    private void DestroyModel()
    {
        if (modelTransform != null)
        {
            Destroy(modelTransform.gameObject);
        }
    }

    private void OnDestroy()
    {
        if (trailInstance != null)
        {
            Destroy(trailInstance, 5f);
        }
    }
}
