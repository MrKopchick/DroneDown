using UnityEngine;
using System.Collections.Generic;
using Game.Construction;

namespace Game.AA.Defenses
{
    public class MissileAAStation : AAStationBase
    {
        [SerializeField] private GameObject missilePrefab;
        [SerializeField] private Transform missileSpawnPoint;
        
        private MissileStorageManager missileStorageManager;
        private static readonly Dictionary<Transform, MissileAAStation> lockedTargets = new();

        public float MissileReloadTime => reloadTime;
        public int CurrentMissiles => currentAmmo;
        public int MaxMissiles => maxAmmo;
        

        protected override void Awake()
        {
            base.Awake();
            missileStorageManager = FindObjectOfType<MissileStorageManager>();
        }

        protected override void Update()
        {
            base.Update();
            if (!IsUnderConstruction) RefillMissiles();
        }

        private void RefillMissiles()
        {
            if (currentAmmo >= maxAmmo) return;
            
            var storage = missileStorageManager.GetNextStorage();
            if (storage == null) return;
            
            int refillAmount = Mathf.Min(storage.GetCurrentMissiles(), maxAmmo - currentAmmo);
            storage.RemoveMissiles(refillAmount);
            currentAmmo += refillAmount;
        }

        protected override void FindNewTarget()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
            foreach (var hit in hits)
            {
                if (!hit.CompareTag("Shahed") || lockedTargets.ContainsKey(hit.transform)) continue;
                
                LockTarget(hit.transform);
                break;
            }
        }

        private void LockTarget(Transform target)
        {
            lockedTargets[target] = this;
            currentTarget = target;
        }

        private void UnlockTarget()
        {
            if (currentTarget == null || !lockedTargets.TryGetValue(currentTarget, out var station) || station != this) return;
            
            lockedTargets.Remove(currentTarget);
            currentTarget = null;
        }

        protected override void FireWeapon(Vector3 predictedPosition)
        {
            var missile = Instantiate(missilePrefab, missileSpawnPoint.position, 
                Quaternion.LookRotation(currentTarget.position - missileSpawnPoint.position));
            
            var missileComponent = missile.GetComponent<Missile>();
            missileComponent.target = currentTarget;
            
            StartCoroutine(Reload());
        }

        private void OnDestroy()
        {
            UnlockTarget();
        }
    }
}