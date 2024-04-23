using System.Linq;
using OWOGame;
using OWOPluginSimHub.Domain;
using static OWOGame.SensationsFactory;

namespace OWOPluginSimHub.Application
{
    public class Speeddsfas
    {
        static Muscle[] Back => new[] { Muscle.Lumbar_L, Muscle.Lumbar_R, Muscle.Dorsal_L, Muscle.Dorsal_R };
        static Muscle[] Abdominal => new[] { Muscle.Abdominal_L, Muscle.Abdominal_R, };
        static Muscle[] Pectorals => new[] { Muscle.Pectoral_L, Muscle.Pectoral_R };

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

        public MicroSensation Sensation() => Create(100, 1f, 80);
    }
}