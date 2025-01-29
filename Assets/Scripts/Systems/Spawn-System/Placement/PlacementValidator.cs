using UnityEngine;
using Game.Spawning.Data;
using Game.Construction;

namespace Game.Spawning.Placement
{
    public sealed class PlacementValidator
    {
        private const float MaxSlopeAngle = 30f;
        private const float MinObjectDistance = 5f;
        private const float MaxHeightDifference = 2f;
        private const float RaycastHeightOffset = 10f;
        private const float RaycastDistance = 20f;
        
        private readonly LayerMask placementMask;

        public PlacementValidator(LayerMask mask) => placementMask = mask;

        public bool IsPositionValid(Vector3 position, SpawnableObject spawnable)
        {
            if (!ValidateGround(position, out var hit)) return false;
            if (!ValidateSlope(hit.normal)) return false;
            if (!ValidateProximity(position, spawnable)) return false;
            return ValidateHeightDifference(position, hit.point);
        }

        private bool ValidateGround(Vector3 position, out RaycastHit hit)
        {
            return Physics.Raycast(
                position + Vector3.up * RaycastHeightOffset, 
                Vector3.down, 
                out hit, 
                RaycastDistance, 
                placementMask
            );
        }

        private bool ValidateSlope(Vector3 normal)
        {
            return Vector3.Angle(normal, Vector3.up) <= MaxSlopeAngle;
        }

        private bool ValidateProximity(Vector3 position, SpawnableObject spawnable)
        {
            if (spawnable.isTarget) return true;
            
            Collider[] colliders = Physics.OverlapSphere(
                position, 
                MinObjectDistance
            );
            
            foreach (var collider in colliders)
            {
                if (collider.CompareTag("SpawnedObject") && 
                   !collider.GetComponent<ConstructionProcess>())
                {
                    return false;
                }
            }
            return true;
        }

        private bool ValidateHeightDifference(Vector3 position, Vector3 groundPoint)
        {
            return Mathf.Abs(position.y - groundPoint.y) <= MaxHeightDifference;
        }
    }
}