using UnityEngine;
using System.Collections.Generic;

namespace RadarSystem{
    public class RadarManager
    {
        private List<Radar> radars;

        public RadarManager()
        {
            radars = new List<Radar>();
        }

        public void AddRadar(Radar radar)
        {
            radars.Add(radar);
        }

        public void Update(float deltaTime)
        {
            foreach (var radar in radars)
            {
                radar.Update(deltaTime);
            }
        }

    }

}