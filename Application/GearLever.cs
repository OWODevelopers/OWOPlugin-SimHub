using System.Threading.Tasks;

namespace OWOPlugin
{
    public class GearLever
    {
        readonly HapticSystem hapticSystem;
    
        string lastGear;
        public bool IsShiftingGear { get; private set; }

        public GearLever(HapticSystem hapticSystem)
        {
            this.hapticSystem = hapticSystem;
        }

        public void Update(WorldContext data)
        {
            if (data.Gear == lastGear) return;
            lastGear = data.Gear;
        
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