using System.Collections;
using UnityEngine;

namespace RadarSystem{
    public class Target
    {
        public Vector3 Position { get; set; }
        public bool IsDetected { get; set; }

        public Target(Vector3 position)
        {
            Position = position;
            IsDetected = false;
        }
    }

}