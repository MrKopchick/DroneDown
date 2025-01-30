using UnityEngine;

namespace Game.AA.Defenses
{
    public class GunAAStation : AAStationBase
    {
        [Header("Gun Configuration")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform turretBase;
        [SerializeField] private Transform[] gunBarrels;
        [SerializeField] private float fireRate = 2f;
        [SerializeField] private float rotationSpeed = 120f;
        [SerializeField] private float bulletSpeed = 50f;

        private float fireCooldown;
        private Vector3 currentAimPosition;

        public float FireRate => fireRate;
        public float BulletSpeed => bulletSpeed;
        public float FireRadius => detectionRadius;

        protected override void Update()
        {
            base.Update();
            
            if (currentTarget != null)
            {
                currentAimPosition = CalculatePredictedPosition();
                UpdateTurret();
            }
            
            fireCooldown -= Time.deltaTime;
        }

        private void UpdateTurret()
        {
            RotateTurret(currentAimPosition);
            
            if (fireCooldown <= 0 && IsAimedAtTarget(currentAimPosition))
            {
                FireWeapon(currentAimPosition);
                fireCooldown = 1f / fireRate;
            }
        }

        private void RotateTurret(Vector3 targetPosition)
        {
            Vector3 horizontalDirection = targetPosition - turretBase.position;
            horizontalDirection.y = 0;
            
            if (horizontalDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(horizontalDirection);
                turretBase.rotation = Quaternion.RotateTowards(
                    turretBase.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }

            Vector3 verticalDirection = targetPosition - turretBase.position;
            foreach (var barrel in gunBarrels)
            {
                float angle = Mathf.Atan2(verticalDirection.y, horizontalDirection.magnitude) * Mathf.Rad2Deg;
                barrel.localRotation = Quaternion.Euler(-angle, 0, 0);
            }
        }

        protected override void FireWeapon(Vector3 predictedPosition)
        {
            foreach (var barrel in gunBarrels)
            {
                GameObject bullet = Instantiate(
                    bulletPrefab,
                    barrel.position,
                    barrel.rotation
                );
                
                Bullet bulletComponent = bullet.GetComponent<Bullet>();
                if (bulletComponent != null)
                {
                    bulletComponent.Initialize(bulletSpeed);
                }
            }

            CameraShake.Instance?.Shake(0.2f, 0.1f);
        }

        private bool IsAimedAtTarget(Vector3 targetPosition)
        {
            Vector3 direction = (targetPosition - turretBase.position).normalized;
            return Vector3.Angle(turretBase.forward, direction) < 15f;
        }
    }
}