namespace StatisticalLanguageReconstruction
{
    public class LanguageStatistics
    {
        private readonly Dictionary<char, Dictionary<char, float>> characterProbabilities = [];
        private readonly HashSet<char> uniqueCharacters = [];

        public IReadOnlySet<char> UniqueCharacters => uniqueCharacters;

        public float GetCharacterProbability(char firstChar, char nextChar)
        {
            if (characterProbabilities.TryGetValue(firstChar, out var nextCharProbabilities))
            {
                if (nextCharProbabilities.TryGetValue(nextChar, out var probability))
                {
                    return probability;
                }
            }

            return 0f;
        }

        public void SetCharacterProbability(char firstChar, char nextChar, float probability)
        {
            uniqueCharacters.Add(firstChar);
            uniqueCharacters.Add(nextChar);

            if (!characterProbabilities.TryGetValue(firstChar, out Dictionary<char, float>? value))
            {
                value = [];
                characterProbabilities[firstChar] = value;
            }

            value[nextChar] = probability;
        }

        public void NormalizeProbabilities()
        {
            foreach (var firstChar in characterProbabilities.Keys)
            {
                var totalProbability = characterProbabilities[firstChar].Values.Sum();
                if (totalProbability > 0)
                {
                    foreach (var nextChar in characterProbabilities[firstChar].Keys.ToList())
                    {
                        characterProbabilities[firstChar][nextChar] /= totalProbability;
                    }
                }
            }
        }
    }
}
