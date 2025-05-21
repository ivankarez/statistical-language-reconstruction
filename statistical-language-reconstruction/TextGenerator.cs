using System.Text;

namespace StatisticalLanguageReconstruction
{
    public class TextGenerator
    {
        private readonly Random random = new Random();

        public string Generate(LanguageStatistics languageStatistics, int characters)
        {
            var stringBuilder = new StringBuilder();

            var currentChar = languageStatistics.UniqueCharacters
                .Where(char.IsLetter)
                .Where(char.IsUpper)
                .OrderBy(_ => random.Next())
                .FirstOrDefault();
            stringBuilder.Append(currentChar);

            var nextCharacterStats = GetInitialNextCharacterStats(languageStatistics, currentChar);

            for (int i = 1; i < characters; i++)
            {
                if (TryGetNextCharacter(nextCharacterStats, out var nextChar))
                {
                    stringBuilder.Append(nextChar);
                    currentChar = nextChar;
                    UpdateNextCharacterStats(languageStatistics, nextCharacterStats, currentChar);
                }
                else
                {
                    break; // No valid next character found
                }
            }

            return stringBuilder.ToString();
        }

        private static Dictionary<char, float> GetInitialNextCharacterStats(LanguageStatistics languageStatistics, char firstChar)
        {
            var nextCharacterStats = new Dictionary<char, float>();

            foreach (var character in languageStatistics.UniqueCharacters)
            {
                nextCharacterStats[character] = languageStatistics.GetCharacterProbability(firstChar, character);
            }

            return nextCharacterStats;
        }

        private static void UpdateNextCharacterStats(LanguageStatistics languageStatistics, Dictionary<char, float> nextCharacterStats,  char nextChar)
        {
            foreach (var character in languageStatistics.UniqueCharacters)
            {
                var newProbability = languageStatistics.GetCharacterProbability(nextChar, character);
                nextCharacterStats[character] = newProbability;
            }
        }

        private bool TryGetNextCharacter(Dictionary<char, float> probabilities, out char nextChar)
        {
            var filteredProbs = probabilities
                .Where(kvp => kvp.Value > 0)
                .ToDictionary();

            var probabilitySum = filteredProbs.Values.Sum();
            var randomValue = random.NextDouble() * probabilitySum;
            var cumulativeProbability = 0.0;

            foreach (var kvp in filteredProbs)
            {
                cumulativeProbability += kvp.Value;
                if (randomValue < cumulativeProbability)
                {
                    nextChar = kvp.Key;
                    return true;
                }
            }

            nextChar = default;
            return false;
        }
    }
}
