using System.Threading.Tasks;
using OWOPluginSimHub.Domain;

namespace OWOPluginSimHub.Application
{
    public class GearLever
    {
        readonly HapticSystem hapticSystem;
    
        string lastGear = "0";
        public bool IsShiftingGear { get; private set; }

        public GearLever(HapticSystem hapticSystem)
        {
            this.hapticSystem = hapticSystem;
        }

        public void Update(WorldContext data)
        {
            if (data.Gear == lastGear) return;
            lastGear = data.Gear;
        
            FeelGearChange(data);
        }

        void FeelGearChange(WorldContext data)
        {
            if (data.Brake > 0 || IsShiftingGear) return;

            hapticSystem.Stop();
            _ = ShiftGear();
        }

        async Task ShiftGear()
        {
            IsShiftingGear = true;
            await Task.Delay(300);
            IsShiftingGear = false;
        }
    }
}