namespace RadarSystem{
    namespace Data{
        public interface IRadarDataHandler
        {
            ProcessedRadarData ProcessDetectionResult(RadarDetectionResult result);
        }
    }
    
}