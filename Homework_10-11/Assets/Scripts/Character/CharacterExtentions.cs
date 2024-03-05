using System.Linq;

namespace Sample
{
    public static class CharacterExtentions
    {
        public static void Reset(this Character character)
        {
            var stats = character.GetStats();

            foreach (var stat in stats)
            {
                character.RemoveStat(stat.Key, stat.Value);
            }
        }

        public static bool TryGetStat(this Character character, string statName, out int result)
        {
            var stats = character.GetStats();

            var pair = stats.FirstOrDefault(s => s.Key == statName);
            result = pair.Value;

            if (pair.Key == null)
            {
                return false;
            }

            return true;
        }
    }
}
