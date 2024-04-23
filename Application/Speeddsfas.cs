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

        int BrakeIntensity(WorldContext context) => context.Brake > 0 ? (int)(SpeedIntensity(context) * 1.25f) : 0;

        public Speeddsfas(HapticSystem hapticSystem)
        {
            this.hapticSystem = hapticSystem;
        }

        public void Update(WorldContext context)
        {
            steeringMuscles.AccelerationX = context.AccelerationX;

            var accelerationMuscles = SpeedMuscles.WithIntensity(SpeedIntensity(context));
            var abdominal = Abdominal.WithIntensity((int)(SpeedIntensity(context) / 2f));
            var brakeMuscles = BrakeMuscles.WithIntensity((int)Plugin.Clamp(BrakeIntensity(context), 0, 70));
            var allMuscles = accelerationMuscles.Concat(abdominal).Concat(brakeMuscles).ToList();

            var sensation = SensationsFactory.Create(100, 1f, 80);
            var muscles = steeringMuscles.ApplyDirectionForce(allMuscles);

            hapticSystem.Send(sensation, muscles);
        }

        public int SpeedIntensity(WorldContext context)
        {
            if (context.KmHour <= 5) return 0;
            if (context.Brake > 0) return (int)(context.KmHour / 3f);
            switch (context.Gear)
            {
                case "0":
                    return context.KmHour;
                case "1":
                    return 15 + context.KmHour;
                case "2":
                    return (int)(20 + context.KmHour / 1.3f);
                case "3":
                    return (int)(30 + context.KmHour / 1.6f);
                default:
                    return (int)(context.KmHour / 3f);
            }
        }
    }
}