using OWOGame;

namespace OWOPlugin
{
    public interface HapticSystem
    {
        bool IsConnected { get; }
        void Send(Sensation sensation, Muscle[] muscles);
        void Stop();
        void Connect();
    }
}