using System.Globalization;
using System.Reflection;
using System.Text.Json;

namespace BookGen.RakeEngine.Internals;

//https://github.com/6/stopwords-json
internal static class StopwordsLoader
{
    public static string[] GetStopWords(CultureInfo culture)
    {
        string name = $"{culture.TwoLetterISOLanguageName}.json";

        Assembly current = typeof(StopwordsLoader).GetTypeInfo().Assembly;

        string[]? resources = current.GetManifestResourceNames();

        if (resources == null)
            throw new InvalidOperationException("No stopwords found");

        string file = resources.First(s => s.EndsWith(name, StringComparison.CurrentCultureIgnoreCase));

        using (Stream? stream = current.GetManifestResourceStream(file))
        {
            if (stream == null)
                throw new InvalidOperationException($"No stopword can be found for language: {culture}");

            using (StreamReader reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd();

                var deserialized = JsonSerializer.Deserialize<string[]>(json);
                
                if (deserialized == null)
                    throw new InvalidOperationException($"No stopword can be found for language: {culture}");

                return deserialized;

            }
        }

        throw new InvalidOperationException($"No stopword can be found for language: {culture}");
    }
}