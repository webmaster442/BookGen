using BookGen.Cli.Annotations;
using System.Reflection;

namespace BookGen.Cli.ArgumentParsing
{
    internal static class Autocomplete
    {
        public static IEnumerable<string> GetInfo<TArguments>() where TArguments : ArgumentsBase
        {
            var properties = typeof(TArguments)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var sw = property.GetCustomAttribute<SwitchAttribute>();
                if (sw != null)
                {
                    yield return $"-{sw.ShortName}";
                    yield return $"--{sw.LongName}";
                }
            }


        }
    }
}
