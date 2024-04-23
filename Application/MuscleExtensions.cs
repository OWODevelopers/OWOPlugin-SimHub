using System.Collections.Generic;
using System.Linq;
using OWOGame;

namespace OWOPlugin
{
    public static class MuscleExtensions
    {
        public static IEnumerable<Muscle> ApplyIntensityMultiplier(this IEnumerable<Muscle> allMuscles, double percentage, Muscle[] applyIn)
        {
            var result = allMuscles.ToList();
            applyIn.ToList().ForEach(
                muscle =>
                {
                    if (!allMuscles.Any(x => x.id == muscle.id)) return;
                        
                    var muscleToCalibrate = result.First(m => m.id == muscle.id);
                    result.Remove(muscleToCalibrate);
                    result.Add(muscleToCalibrate.WithIntensity((int)(muscleToCalibrate.intensity * percentage)));
                });

            return result;
        }

        public static bool Contains(this IEnumerable<Muscle> allMuscles, params Muscle[] searchingFor)
        {
            var result = true;
            searchingFor.ToList().ForEach(muscle => result &= allMuscles.Any(x => x.id == muscle.id));
            return result;
        }

        public static Muscle Find(this IEnumerable<Muscle> allMuscles, Muscle muscle)
            => allMuscles.First(x => x.id == muscle.id);
    }
}