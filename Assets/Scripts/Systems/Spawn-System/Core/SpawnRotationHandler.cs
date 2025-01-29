using Game.Spawning.Data;
using Game.Spawning.Preview;
using UnityEngine;
using Game.Spawning.Placement;

namespace Game.Spawning.Controllers
{
    public sealed class SpawnRotationHandler
    {
        private readonly ISpawnController spawnController;
        private const float RotationStep = 90f;

        public SpawnRotationHandler(ISpawnController controller)
        {
            spawnController = controller;
        }

        public void HandleRotationInput()
        {
            if (Input.GetKeyDown(KeyCode.R))
                spawnController.RotatePreview(RotationStep);
        }
    }
}