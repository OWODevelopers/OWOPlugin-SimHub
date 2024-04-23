using OWOGame;

namespace OWOPluginSimHub.Application
{
    public interface HapticSystem
    {
        void Send(Sensation sensation, Muscle[] muscles);
        void Stop();
    }
}