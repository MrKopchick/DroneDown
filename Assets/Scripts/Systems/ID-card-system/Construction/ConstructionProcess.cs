using UnityEngine;

namespace Game.Construction
{
    [RequireComponent(typeof(GenericIDCard))]
    public sealed class ConstructionProcess : MonoBehaviour
    {
        private float baseTime;
        private float remainingTime;
        private float currentSpeed = 1f;
        private bool isActive;

        public float Progress => 1f - Mathf.Clamp01(remainingTime / baseTime);

        private void Awake() => InitializeFromIDCard();

        private void InitializeFromIDCard()
        {
            var idCard = GetComponent<GenericIDCard>();
            if (idCard?.spawnableObject != null)
            {
                baseTime = idCard.spawnableObject.baseConstructionTime;
                remainingTime = baseTime;
            }
        }

        public void StartBuilding(float speed)
        {
            currentSpeed = speed;
            isActive = true;
        }

        public void UpdateBuildingSpeed(float speed) => currentSpeed = speed;

        private void Update()
        {
            if (!isActive) return;
            
            remainingTime -= Time.deltaTime * currentSpeed;
            
            if (remainingTime <= 0)
            {
                CompleteConstruction();
            }
        }

        private void CompleteConstruction()
        {
            isActive = false;
            remainingTime = 0f;
            ConstructionQueue.Instance.NotifyComplete();
        }
    }
}