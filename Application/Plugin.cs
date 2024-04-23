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

        readonly ImpactSensor impactSensor = new ImpactSensor();
        readonly GearLever lever;
        readonly Speeddsfas speeddsfas;

        public Plugin(HapticSystem hapticSystem)
        {
            this.hapticSystem = hapticSystem;

            lever = new GearLever(hapticSystem);
            speeddsfas = new Speeddsfas();
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

            hapticSystem.Send(DrivingSensation(), speeddsfas.MusclesFrom(Data));
        }

        static MicroSensation DrivingSensation() 
            => SensationsFactory.Create(100, 1f, 80);

        public static float Clamp(float value, float min, float max) => Math.Max(min, Math.Min(max, value));
    }
}