using System.Linq;
using OWOGame;
using OWOPluginSimHub.Domain;
using Math = System.Math;

namespace OWOPluginSimHub.Application
{
    public class Plugin
    {
        public WorldContext Data { get; set; }

        readonly HapticSystem hapticSystem;

        int KmPerHour => (int)Data.Speed;
        public int SpeedIntensity
        {
            get
            {
                if (KmPerHour <= 5) return 0;
                if (IsPushingBrake) return (int)(KmPerHour / 3f);
                switch (Data.Gear)
                {
                    case "0":
                        return KmPerHour;
                    case "1":
                        return 15 + KmPerHour;
                    case "2":
                        return (int)(20 + KmPerHour / 1.3f);
                    case "3":
                        return (int)(30 + KmPerHour / 1.6f);
                    default:
                        return (int)(KmPerHour / 3f);
                }
            }
        }
        int BrakeIntensity => IsPushingBrake ? (int)(SpeedIntensity * 1.25f) : 0;
        bool IsPushingBrake => Data.Brake > 0;

        static Muscle[] SpeedMuscles => new[]
            { Muscle.Lumbar_L, Muscle.Lumbar_R, Muscle.Dorsal_L, Muscle.Dorsal_R };

        static Muscle[] Abdominal => new[] { Muscle.Abdominal_L, Muscle.Abdominal_R, };

        static Muscle[] BrakeMuscles => new[] { Muscle.Pectoral_L, Muscle.Pectoral_R };

        readonly SteeringMusclesBuilder steeringMuscles = new SteeringMusclesBuilder();
        readonly ImpactSensor impactSensor = new ImpactSensor();
        readonly GearLever lever;

        public Plugin(HapticSystem hapticSystem)
        {
            this.hapticSystem = hapticSystem;
        
            lever = new GearLever(hapticSystem);
        }

        public void UpdateFeelingBasedOnWorld()
        {
            if (!Data.IsRaceOn)
            {
                impactSensor.Reset();
                hapticSystem.Stop();
                return;
            }

            impactSensor.Update(KmPerHour);
            if (impactSensor.DidImpact)
            {
                hapticSystem.Send(Sensation.Ball, Muscle.All.WithIntensity(impactSensor.Intensity()));
                return;
            }

            lever.Update(Data);
            if (lever.IsShiftingGear)
                return;

            SendSensation();
        }

        void SendSensation()
        {
            steeringMuscles.AccelerationX = Data.AccelerationX;
            
            var accelerationMuscles = SpeedMuscles.WithIntensity(SpeedIntensity);
            var abdominal = Abdominal.WithIntensity((int)( SpeedIntensity/2f));
            var brakeMuscles = BrakeMuscles.WithIntensity((int)Clamp(BrakeIntensity, 0, 70));
            var allMuscles = accelerationMuscles.Concat(abdominal).Concat(brakeMuscles).ToList();

            var sensation = SensationsFactory.Create(100, 1f, 80);
            var muscles = steeringMuscles.ApplyDirectionForce(allMuscles);

            hapticSystem.Send(sensation, muscles);
        }

        public static float Clamp(float value, float min, float max) => Math.Max(min, Math.Min(max, value));
    }
}