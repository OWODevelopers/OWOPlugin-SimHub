using OWOPlugin;
using OWOGame;

public class MockHapticSystem : HapticSystem
{
    public Sensation Last;

    public bool IsConnected { get; private set; }

    public void Send(Sensation sensation, Muscle[] muscles)
    {
        Last = sensation;
    }

    public void Stop()
    {
    }

    public void Connect()
    {
        IsConnected = true;
    }
}