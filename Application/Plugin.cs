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
                if (Data.Brake > 0) return (int)(KmPerHour / 3f);
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

        readonly ImpactSensor impactSensor = new ImpactSensor();
        readonly GearLever lever;
        readonly Speeddsfas speeddsfas;

        public Plugin(HapticSystem hapticSystem)
        {
            this.hapticSystem = hapticSystem;

            lever = new GearLever(hapticSystem);
            speeddsfas = new Speeddsfas(hapticSystem);
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

            speeddsfas.Update(Data);
        }

        public static float Clamp(float value, float min, float max) => Math.Max(min, Math.Min(max, value));
    }
}