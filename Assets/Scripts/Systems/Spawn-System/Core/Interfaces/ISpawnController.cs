using Game.Spawning.Data;

namespace Game.Spawning.Controllers
{
    public interface ISpawnController
    {
        void SetActiveSpawnable(SpawnableObject spawnable);
        void CancelSpawning();
        void RotatePreview(float degrees);
    }
}