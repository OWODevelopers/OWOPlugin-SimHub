using OWOGame;

namespace OWOPlugin
{
    public class OWOHaptic : HapticSystem
    {
        public bool IsConnected { get; }

        public void Send(Sensation sensation, Muscle[] muscles)
        {
            OWO.Send(sensation, muscles);
        }

        public void Stop()
        {
            OWO.Stop();
        }

        public void Connect()
        {
            OWO.AutoConnect();
        }
    }
}