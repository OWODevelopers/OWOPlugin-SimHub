using System.Linq;
using OWOGame;
using OWOPluginSimHub.Domain;
using static OWOGame.Muscle;
using static OWOGame.SensationsFactory;

namespace OWOPluginSimHub.Application
{
    public class Speeddsfas
    {
        static Muscle[] Back => new[] { Lumbar_L, Lumbar_R, Dorsal_L, Dorsal_R };
        static Muscle[] Abdominal => new[] { Abdominal_L, Abdominal_R, };
        static Muscle[] Pectorals => new[] { Pectoral_L, Pectoral_R };

        readonly SteeringMusclesBuilder steeringMuscles = new SteeringMusclesBuilder();
        readonly SpeedIntensity speedIntensity = new SpeedIntensity();

        int BrakeIntensity(WorldContext context) => context.Brake > 0 ? (int)(speedIntensity.From(context) * 1.25f) : 0;

        public Muscle[] MusclesFrom(WorldContext context)
        {
            var backInSeat = Back.WithIntensity(speedIntensity.From(context));
            var front = Abdominal.WithIntensity((int)(speedIntensity.From(context) / 2f));
            var brakeMuscles = Pectorals.WithIntensity((int)Plugin.Clamp(BrakeIntensity(context), 0, 70));
            var allMuscles = backInSeat.Concat(front).Concat(brakeMuscles).ToList();

            steeringMuscles.AccelerationX = context.AccelerationX;
            return steeringMuscles.ApplyDirectionForce(allMuscles);
        }
    }
}