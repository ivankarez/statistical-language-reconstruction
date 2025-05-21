namespace StatisticalLanguageReconstruction
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var analizer = new LanguageAnalizer();
            var statistics = analizer.GetAnalisis("sample.txt");

            var generator = new TextGenerator();
            var generatedText = generator.Generate(statistics, 100);
            Console.WriteLine("Generated Text:");
            Console.WriteLine(generatedText);
        }
    }
}
