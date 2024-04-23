using OWOGame;
using OWOPluginSimHub.Domain;
using static OWOGame.Sensation;
using static OWOGame.SensationsFactory;

namespace OWOPluginSimHub.Application
{
    public class Plugin
    {
        public WorldContext Data { get; set; }

        readonly HapticSystem hapticSystem;
        readonly ImpactSensor impactSensor = new ImpactSensor();
        readonly GearLever lever;
        readonly DrivingMusclesBuilder driving;

        public Plugin(HapticSystem hapticSystem)
        {
            this.hapticSystem = hapticSystem;

            lever = new GearLever(hapticSystem);
            driving = new DrivingMusclesBuilder();
        }

        public void UpdateFeelingBasedOnWorld()
        {
            if (!Data.IsRaceOn)
            {
                impactSensor.Reset();
                hapticSystem.Stop();
                return;
            }

            if (TryFeelImpact()) return;
            if (TryFeelGearShift()) return;

            FeelDriving();
        }

        void FeelDriving() => hapticSystem.Send(DrivingSensation(), driving.MusclesFrom(Data));

        bool TryFeelGearShift()
        {
            lever.Update(Data);
            return lever.IsShiftingGear;
        }

        bool TryFeelImpact()
        {
            impactSensor.Update(Data.KmHour);
            if (impactSensor.DidImpact) 
                hapticSystem.Send(Ball, impactSensor.Muscles());
            
            return impactSensor.DidImpact;
        }

        static MicroSensation DrivingSensation() => Create(100, 1f, 80);
    }
}