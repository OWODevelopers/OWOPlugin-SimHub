using OWOPluginSimHub.Domain;

namespace OWOPluginSimHub.Application
{
    public class SpeedIntensity
    {
        public int From(WorldContext context)
        {
            if (context.KmHour <= 5) return 0;
            if (context.Brake > 0) return (int)(context.KmHour / 3f);
            
            switch (context.Gear)
            {
                case "0":
                    return context.KmHour;
                case "1":
                    return 15 + context.KmHour;
                case "2":
                    return (int)(20 + context.KmHour / 1.3f);
                case "3":
                    return (int)(30 + context.KmHour / 1.6f);
                default:
                    return (int)(context.KmHour / 3f);
            }
        }
    }
}