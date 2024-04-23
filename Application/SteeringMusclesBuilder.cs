using System;
using System.Collections.Generic;
using System.Linq;
using OWOGame;
using Math = System.Math;

namespace OWOPlugin
{
    public class SteeringMusclesBuilder
    {
        public float AccelerationX { get; set; }
        float SteerIntensity => Plugin.Clamp((Math.Abs(AccelerationX) - 3) / 10f, 0.1f, 0.3f);

        static Muscle[] RightMuscles => new[]
            { Muscle.Pectoral_R, Muscle.Abdominal_R, Muscle.Arm_R, Muscle.Lumbar_R, Muscle.Dorsal_R };

        static Muscle[] LeftMuscles => new[]
            { Muscle.Pectoral_L, Muscle.Abdominal_L, Muscle.Arm_L, Muscle.Lumbar_L, Muscle.Dorsal_L };

        bool TurningLeft => AccelerationX < -3;

        Muscle[] SteeringMuscles => TurningLeft ? LeftMuscles :
            AccelerationX > 3 ? RightMuscles : Array.Empty<Muscle>();

        Muscle[] OppositeToSteering => SteeringMuscles.SequenceEqual(RightMuscles) ? LeftMuscles : RightMuscles;

        public Muscle[] ApplyDirectionForce(IEnumerable<Muscle> allMuscles)
        {
            if (allMuscles.Contains(Muscle.Arm_L, Muscle.Arm_R))
                throw new ArgumentException("Muscles collection cannot contain arm muscles before applying turn");
            if (SteeringMuscles.Length == 0)
                return allMuscles.ToArray();

            return AppendArms(allMuscles)
                .ApplyIntensityMultiplier(1 + SteerIntensity, SteeringMuscles)
                .ApplyIntensityMultiplier(1 - SteerIntensity, OppositeToSteering)
                .ToArray();
        }

        IEnumerable<Muscle> AppendArms(IEnumerable<Muscle> allMuscles)
        {
            var muscleToAppend = TurningLeft ? Muscle.Arm_L : Muscle.Arm_R;

            return allMuscles.Append(muscleToAppend.WithIntensity(allMuscles.Find(Muscle.Lumbar_R).intensity));
        }
    }
}