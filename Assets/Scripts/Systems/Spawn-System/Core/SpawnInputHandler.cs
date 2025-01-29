namespace Game.Spawning.Controllers
{
    using UnityEngine;

    public sealed class SpawnInputHandler
    {
        private readonly ISpawnController spawnController;

        public SpawnInputHandler(ISpawnController controller)
        {
            spawnController = controller;
        }

        public void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                spawnController.RotatePreview(90f);
            }
        }
    }
}