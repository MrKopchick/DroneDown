using UnityEngine;
using Game.Construction;

namespace Game
{
    public class Factory : MonoBehaviour
    {
        public float buildBoost = 1.0f;

        private void Start()
        {
            ConstructionQueue.Instance.RegisterFactory(this);
        }

        private void OnDestroy()
        {
            ConstructionQueue.Instance.UnregisterFactory(this); 
        }
    }
}