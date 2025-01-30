using UnityEngine;
using System.Collections;
using Game.Construction;


namespace Game.AA.Defenses
{
    public abstract class AAStationBase : MonoBehaviour
    {
        [SerializeField] protected float detectionRadius = 50f;
        [SerializeField] protected float reloadTime = 2f;
        [SerializeField] protected int maxAmmo = 10;
        
        protected int currentAmmo;
        protected bool isReloading;
        protected Transform currentTarget;
        protected ConstructionProcess constructionProcess;

        public bool IsUnderConstruction => constructionProcess != null && constructionProcess.Progress < 1f;
        public int CurrentAmmo => currentAmmo;
        public int MaxAmmo => maxAmmo;
        public float ReloadTime => reloadTime;
        public float DetectionRadius => detectionRadius;

        protected virtual void Awake()
        {
            constructionProcess = GetComponent<ConstructionProcess>();
            currentAmmo = maxAmmo;
        }

        protected virtual void Update()
        {
            if (IsUnderConstruction) return;
            
            UpdateTarget();
            TryFire();
            HandleReload();
        }

        protected virtual void UpdateTarget()
        {
            if (!HasValidTarget()) FindNewTarget();
        }

        protected virtual bool HasValidTarget()
        {
            return currentTarget != null && Vector3.Distance(transform.position, currentTarget.position) <= detectionRadius;
        }

        protected virtual void FindNewTarget()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Shahed"))
                {
                    currentTarget = hit.transform;
                    break;
                }
            }
        }

        protected virtual void TryFire()
        {
            if (currentTarget == null || !CanFire()) return;
            
            FireWeapon(CalculatePredictedPosition());
            DecreaseAmmo();
        }

        protected abstract void FireWeapon(Vector3 predictedPosition);

        protected virtual Vector3 CalculatePredictedPosition()
        {
            if (currentTarget == null) return Vector3.zero;
            
            Rigidbody targetRb = currentTarget.GetComponent<Rigidbody>();
            return targetRb != null ? 
                PredictMovingTarget(targetRb) : 
                currentTarget.position;
        }

        protected virtual Vector3 PredictMovingTarget(Rigidbody targetRb)
        {
            float distance = Vector3.Distance(transform.position, currentTarget.position);
            float predictionTime = distance / 20f;
            return currentTarget.position + targetRb.velocity * predictionTime;
        }

        protected virtual bool CanFire()
        {
            return currentAmmo > 0 && !isReloading;
        }

        protected virtual void HandleReload()
        {
            if (currentAmmo > 0 || isReloading) return;
            
            StartCoroutine(Reload());
        }

        protected virtual IEnumerator Reload()
        {
            isReloading = true;
            yield return new WaitForSeconds(reloadTime);
            currentAmmo = maxAmmo;
            isReloading = false;
        }

        protected virtual void DecreaseAmmo()
        {
            currentAmmo = Mathf.Max(currentAmmo - 1, 0);
        }
    }
}