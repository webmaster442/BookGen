using BookGen.Cli;
using BookGen.Vfs;

namespace BookGen;

internal static class Extensions
{
    extension(IValidationContext context)
    {
        public bool IsValidTemplateFile(string templateFile)
        {
            if (string.IsNullOrEmpty(templateFile))
                return true;

            var source = context.Resolve<IAssetSource>();

            if (source.AssetNames.Contains(templateFile))
                return true;

            return context.FileSystem.FileExists(templateFile);
        }
    }
}
