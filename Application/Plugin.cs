using OWOGame;
using OWOPluginSimHub.Domain;
using static OWOGame.Sensation;
using static OWOGame.SensationsFactory;

namespace OWOPluginSimHub.Application
{
    public class Plugin
    {
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

        public void UpdateFeelingBasedOnWorld(WorldContext data)
        {
            if (!data.IsRaceOn)
            {
                impactSensor.Reset();
                hapticSystem.Stop();
                return;
            }

            if (TryFeelImpact(data)) return;
            if (TryFeelGearShift(data)) return;

            FeelDriving(data);
        }

        void FeelDriving(WorldContext data) => hapticSystem.Send(DrivingSensation(), driving.MusclesFrom(data));

        bool TryFeelGearShift(WorldContext data)
        {
            lever.Update(data);
            return lever.IsShiftingGear;
        }

        bool TryFeelImpact(WorldContext data)
        {
            impactSensor.Update(data.KmHour);
            if (impactSensor.DidImpact) 
                hapticSystem.Send(Ball, impactSensor.Muscles());
            
            return impactSensor.DidImpact;
        }

        static MicroSensation DrivingSensation() => Create(100, 1f, 80);
    }
}