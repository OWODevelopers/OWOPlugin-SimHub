using OWOPluginSimHub.Domain;

public static class WorldContextBuilder
{
    public static WorldContext DuringRace(float speed = 0) => new WorldContext() { IsRaceOn = true, Speed = speed };
    public static WorldContext OutsideRace() => new WorldContext() { IsRaceOn = false};
}