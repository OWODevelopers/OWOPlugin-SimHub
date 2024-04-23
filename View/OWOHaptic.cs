using OWOGame;
using OWOPluginSimHub.Application;

namespace OWOPluginSimHub.View
{
    public class OWOHaptic : HapticSystem
    {
        public void Send(Sensation sensation, Muscle[] muscles)
        {
            OWO.Send(sensation, muscles);
        }

        public void Stop()
        {
            OWO.Stop();
        }
    }
}