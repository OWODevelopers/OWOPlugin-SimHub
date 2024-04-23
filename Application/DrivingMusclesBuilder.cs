using System.Linq;
using OWOGame;
using OWOPluginSimHub.Domain;
using static OWOGame.Muscle;

namespace OWOPluginSimHub.Application
{
    public class DrivingMusclesBuilder
    {
        static Muscle[] Back => new[] { Lumbar_L, Lumbar_R, Dorsal_L, Dorsal_R };
        static Muscle[] Abdominal => new[] { Abdominal_L, Abdominal_R, };
        static Muscle[] Pectorals => new[] { Pectoral_L, Pectoral_R };

        readonly SteeringMusclesBuilder steeringMuscles = new SteeringMusclesBuilder();

        public Muscle[] MusclesFrom(WorldContext context)
        {
            var front = Abdominal.WithIntensity((int)(SpeedIntensity.From(context) / 2f));
            var brakeMuscles = Pectorals.WithIntensity((int)Math.Clamp(BrakeIntensity(context), 0, 70));
            var backInSeat = Back.WithIntensity(SpeedIntensity.From(context));
            var allMuscles = backInSeat.Concat(front).Concat(brakeMuscles).ToList();

            steeringMuscles.AccelerationX = context.AccelerationX;
            return steeringMuscles.ApplyDirectionForce(allMuscles);
        }

        int BrakeIntensity(WorldContext context) => context.Brake > 0 ? (int)(SpeedIntensity.From(context) * 1.25f) : 0;
    }
}