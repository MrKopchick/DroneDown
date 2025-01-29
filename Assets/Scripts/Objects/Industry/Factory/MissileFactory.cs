using UnityEngine;
namespace Game.Industry{
    public class MissileFactory : MonoBehaviour
    {
        [Header("Factory Settings")]
        public float productionTime = 5f;
        public int missilesPerBatch = 1;

        private float productionTimer = 0f;
        private MissileStorageManager storageManager;

        private void Start()
        {
            storageManager = new MissileStorageManager();
        }

        private void Update()
        {
            productionTimer += Time.deltaTime;

            if (productionTimer >= productionTime)
            {
                ProduceMissiles();
                productionTimer = 0f;
            }
        }

        private void ProduceMissiles()
        {

            var targetStorage = storageManager.GetNextStorage();
            if (targetStorage == null)
            {
                Debug.LogWarning("No available storage to deliver missiles");
                return;
            }

            targetStorage.AddMissiles(missilesPerBatch);
            Debug.Log($"Delivered {missilesPerBatch} missiles to {targetStorage.name}");
        }
    }
}