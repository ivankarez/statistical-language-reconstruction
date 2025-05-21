namespace StatisticalLanguageReconstruction
{
    public class LanguageAnalizer
    {
        public LanguageStatistics GetAnalisis(string sampleFilePath)
        {
            // Read the file content
            string text = File.ReadAllText(sampleFilePath);

            var prevChar = text[0];
            var statistics = new LanguageStatistics();

            for (int i = 1; i < text.Length; i++)
            {
                var currentChar = text[i];
                var existingProbability = statistics.GetCharacterProbability(prevChar, currentChar);
                statistics.SetCharacterProbability(prevChar, currentChar, existingProbability + 1);

                prevChar = currentChar;
            }

            statistics.NormalizeProbabilities();

            return statistics;
        }
    }
}
