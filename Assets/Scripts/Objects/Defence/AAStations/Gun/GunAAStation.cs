using UnityEngine;
using System.Collections;
using Game.Construction;

namespace Game.AA.Defenses
{
    public class GunAAStation : AAStationBase
    {
        [Header("Gun Settings")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform turretBase;
        [SerializeField] private Transform[] gunPivots;
        [SerializeField] private Transform[] gunBarrels;
        [SerializeField] private float fireRate = 2f;
        [SerializeField] private float rotationSpeed = 120f;
        [SerializeField] private float bulletSpeed = 200f;
        [SerializeField] private float gravity = 4f;

        private float fireCooldown;

        protected  void Start()
        {
            fireCooldown = 0f;
        }

        protected override void Update()
        {
            base.Update();

            if (IsUnderConstruction) return;
            
            if (currentTarget == null) FindNewTarget();

            if (currentTarget != null)
            {
                Vector3 predictedPosition = CalculatePredictedPosition();
                RotateTurretTowards(predictedPosition);

                if (IsTargetInFireRange(predictedPosition))
                {
                    FireWeapon(predictedPosition);
                }
            }

            if (fireCooldown > 0) fireCooldown -= Time.deltaTime;
        }

        protected override Vector3 CalculatePredictedPosition()
        {
            if (currentTarget == null) return Vector3.zero;

            Rigidbody targetRb = currentTarget.GetComponent<Rigidbody>();
            if (targetRb == null) return currentTarget.position;

            Vector3 targetPosition = currentTarget.position;
            Vector3 targetVelocity = targetRb.velocity;
            Vector3 gunPosition = gunBarrels[0].position;

            float bulletTravelTime = 0f;
            Vector3 predictedPosition = targetPosition;

            for (int i = 0; i < 10; i++)
            {
                Vector3 directionToTarget = predictedPosition - gunPosition;
                float distanceToTarget = directionToTarget.magnitude;
                bulletTravelTime = distanceToTarget / bulletSpeed;
                predictedPosition = targetPosition + targetVelocity * bulletTravelTime;
            }

            predictedPosition.y += 1.9f;
            return predictedPosition;
        }

        private void RotateTurretTowards(Vector3 position)
        {
            Vector3 directionToTarget = position - turretBase.position;
            Vector3 horizontalDirection = new Vector3(directionToTarget.x, 0, directionToTarget.z).normalized;

            if (horizontalDirection != Vector3.zero)
            {
                Quaternion horizontalRotation = Quaternion.LookRotation(horizontalDirection);
                turretBase.rotation = Quaternion.RotateTowards(turretBase.rotation, horizontalRotation, rotationSpeed * Time.deltaTime);
            }

            foreach (var gunPivot in gunPivots)
            {
                Vector3 localDirection = turretBase.InverseTransformDirection(position - gunPivot.position);
                float angle = CalculateLaunchAngle(localDirection.magnitude, localDirection.y);
                
                if (angle > 0)
                {
                    Quaternion verticalRotation = Quaternion.Euler(-angle, 0, 0);
                    gunPivot.localRotation = Quaternion.RotateTowards(gunPivot.localRotation, verticalRotation, rotationSpeed * Time.deltaTime);
                }
            }
        }

        private float CalculateLaunchAngle(float distance, float heightDifference)
        {
            float v2 = bulletSpeed * bulletSpeed;
            float g = gravity;

            float discriminant = v2 * v2 - g * (g * distance * distance + 2 * heightDifference * v2);
            if (discriminant < 0) return 0;

            float root = Mathf.Sqrt(discriminant);
            float angle1 = Mathf.Atan((v2 + root) / (g * distance));
            float angle2 = Mathf.Atan((v2 - root) / (g * distance));

            return Mathf.Min(angle1, angle2) * Mathf.Rad2Deg;
        }

        private bool IsTargetInFireRange(Vector3 predictedPosition)
        {
            return Vector3.Distance(turretBase.position, predictedPosition) <= detectionRadius;
        }

        protected override void FireWeapon(Vector3 predictedPosition)
        {
            if (fireCooldown > 0.01f || currentAmmo <= 0) return;

            foreach (var gunBarrel in gunBarrels)
            {
                GameObject bullet = Instantiate(bulletPrefab, gunBarrel.position, gunBarrel.rotation);
                Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
                if (bulletRb != null)
                {
                    Vector3 direction = (predictedPosition - gunBarrel.position).normalized;
                    bulletRb.velocity = direction * bulletSpeed;
                }

                Debug.DrawLine(gunBarrel.position, predictedPosition, Color.green, 0.5f);

                if (CameraShake.Instance != null)
                {
                    CameraShake.Instance.ShakeWithDistance(
                        gunBarrel.position,
                        maxIntensity: 0.2f,
                        maxDistance: 40f,
                        amplitudeMultiplier: 0.02f
                    );
                }
            }

            fireCooldown = 1f / fireRate;
            DecreaseAmmo();
        }

        private void DecreaseAmmo()
        {
            currentAmmo = Mathf.Max(currentAmmo - 1, 0);
            if (currentAmmo <= 0) StartCoroutine(Reload());
        }

        public float GetBulletSpeed() => bulletSpeed;
        public float GetFireRate() => fireRate;
        public float GetFireRadius() => detectionRadius;
    }
}
