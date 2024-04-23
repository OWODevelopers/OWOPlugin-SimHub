using GameReaderCommon;

namespace OWOPluginSimHub.Domain
{
    public struct WorldContext
    {
        public float AccelerationX { get; set; }
        public bool IsRaceOn { get; set; }
        public float Speed { get; set; }
        public int Brake { get; set; }
        public int SurfaceRumbleFrontRight { get; set; }
        public string Gear { get; set; }
        public int CurrentEngineRpm { get; set; }

        public int KmHour => (int)Speed;

        public static WorldContext From(GameData game)
            => new WorldContext
            {
                AccelerationX = (float)game.NewData.AccelerationSway,
                IsRaceOn = !game.GamePaused,
                Speed = (float)game.NewData.SpeedKmh,
                Brake = (int)game.NewData.Brake,
                SurfaceRumbleFrontRight = 1,
                Gear = game.NewData.Gear
            };
    }
}