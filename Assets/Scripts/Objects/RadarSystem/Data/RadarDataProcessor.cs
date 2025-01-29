namespace RadarSystem{
    namespace Data{
        public class RadarDataProcessor : IRadarDataHandler
        {
            public ProcessedRadarData ProcessDetectionResult(RadarDetectionResult result)
            {
                return new ProcessedRadarData(result);
            }
        }
    }
}