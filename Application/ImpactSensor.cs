using System;

namespace OWOPlugin
{
    public class ImpactSensor
    {
        int lastVelocity;
        int speedDifference;
    
        public bool DidImpact => speedDifference > 50;

        public void Update(int newSpeed)
        {
            speedDifference = lastVelocity - newSpeed;
            lastVelocity = newSpeed;
        }

        public int Intensity()
        {
            if (lastVelocity <= 0) return 0;

            if (speedDifference < 50)
                return 0;
            if (speedDifference < 100)
                return 80;
            return 100;
        }

        public void Reset()
        {
            lastVelocity = 0;
        }
    }
}