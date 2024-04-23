using System.Linq;
using OWOGame;
using OWOPluginSimHub.Domain;

namespace OWOPluginSimHub.Application
{
    public class Speeddsfas
    {
        readonly HapticSystem hapticSystem;

        static Muscle[] SpeedMuscles => new[]
            { Muscle.Lumbar_L, Muscle.Lumbar_R, Muscle.Dorsal_L, Muscle.Dorsal_R };

        static Muscle[] Abdominal => new[] { Muscle.Abdominal_L, Muscle.Abdominal_R, };

        static Muscle[] BrakeMuscles => new[] { Muscle.Pectoral_L, Muscle.Pectoral_R };

        readonly SteeringMusclesBuilder steeringMuscles = new SteeringMusclesBuilder();
        readonly SpeedIntensity speedIntensity = new SpeedIntensity();

        int BrakeIntensity(WorldContext context) => context.Brake > 0 ? (int)(speedIntensity.From(context) * 1.25f) : 0;

        public Speeddsfas(HapticSystem hapticSystem)
        {
            this.hapticSystem = hapticSystem;
        }

        public void Update(WorldContext context)
        {
            steeringMuscles.AccelerationX = context.AccelerationX;

            var accelerationMuscles = SpeedMuscles.WithIntensity(speedIntensity.From(context));
            var abdominal = Abdominal.WithIntensity((int)(speedIntensity.From(context) / 2f));
            var brakeMuscles = BrakeMuscles.WithIntensity((int)Plugin.Clamp(BrakeIntensity(context), 0, 70));
            var allMuscles = accelerationMuscles.Concat(abdominal).Concat(brakeMuscles).ToList();

            var sensation = SensationsFactory.Create(100, 1f, 80);
            var muscles = steeringMuscles.ApplyDirectionForce(allMuscles);

            hapticSystem.Send(sensation, muscles);
        }
    }
}