using System.Text;

using Bookgen.Lib.Domain.IO.Configuration;

namespace Bookgen.Lib.Pipeline;

internal static class Extensions
{
    public static StringBuilder AppendH1(this StringBuilder sb, string text)
    {
        sb.AppendLine($"<h1>{text}</h1>");
        return sb;
    }

    public static async Task<string> GetTemplate(this IBookEnvironment environment,
                                                  string? frontMatterTemplate,
                                                  string fallbackTemplate,
                                                  Func<Config, string> defaultTemplateSelector)
    {
        if (!string.IsNullOrEmpty(frontMatterTemplate))
        {
            return await environment.Source.ReadAllTextAsync(frontMatterTemplate);
        }

        var defaultTemplate = defaultTemplateSelector.Invoke(environment.Configuration);

        if (!string.IsNullOrEmpty(defaultTemplate))
        {
            return await environment.Source.ReadAllTextAsync(defaultTemplate);
        }

        return environment.GetAsset(fallbackTemplate);
    }
}
