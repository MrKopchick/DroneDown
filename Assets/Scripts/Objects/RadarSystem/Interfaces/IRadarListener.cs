using RadarSystem.Data;

namespace RadarSystem{
    namespace Interfaces
    {
        public interface IRadarListener
        {
            void OnRadarUpdate(ProcessedRadarData data);
        }
    }
}