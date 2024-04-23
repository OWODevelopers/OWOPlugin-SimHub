using System;
using System.Collections.Generic;
using System.Linq;
using OWOGame;
using OWOPluginSimHub.Application;
using static System.Array;
using static OWOGame.Muscle;

namespace OWOPluginSimHub.Domain
{
    public class SteeringMusclesBuilder
    {
        public float AccelerationX { get; set; }
        float SteerIntensity => Math.Clamp((System.Math.Abs(AccelerationX) - 3) / 10f, 0.1f, 0.3f);
        static Muscle[] RightMuscles => new[] { Pectoral_R, Abdominal_R, Arm_R, Lumbar_R, Dorsal_R };
        static Muscle[] LeftMuscles => new[] { Pectoral_L, Abdominal_L, Arm_L, Lumbar_L, Dorsal_L };
        bool TurningLeft => AccelerationX < -3;
        Muscle[] SteeringMuscles => TurningLeft ? LeftMuscles : AccelerationX > 3 ? RightMuscles : Empty<Muscle>();
        Muscle[] OppositeToSteering => SteeringMuscles.SequenceEqual(RightMuscles) ? LeftMuscles : RightMuscles;
        bool GoingStraight => SteeringMuscles.Length == 0;

        public Muscle[] ApplyDirectionForce(IEnumerable<Muscle> allMuscles)
        {
            if (allMuscles.Contains(Arm_L, Arm_R))
                throw new ArgumentException("Muscles collection cannot contain arm muscles before applying turn");
            
            if (GoingStraight)
                return allMuscles.ToArray();

            return AppendArms(allMuscles)
                .ApplyIntensityMultiplier(1 + SteerIntensity, SteeringMuscles)
                .ApplyIntensityMultiplier(1 - SteerIntensity, OppositeToSteering)
                .ToArray();
        }

        IEnumerable<Muscle> AppendArms(IEnumerable<Muscle> allMuscles) 
            => allMuscles.Append((TurningLeft ? Arm_L : Arm_R)
                .WithIntensity(allMuscles.Find(Lumbar_R).intensity));
    }
}