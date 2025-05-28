using Bookgen.Lib.Domain.IO.Configuration;

using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Migration;

internal sealed class MigrateConfig : IMigrationStep
{
    public async Task<bool> ExecuteAsync(IWritableFileSystem foler, MigrationState state, ILogger logger)
    {
        logger.LogInformation("Migrating configuration...");
        var config = new Config
        {
            BookTitle = state.LegacyConfig.Metadata.Title,
            TocFile = FileNameConstants.TableOfContents,
            OutputFolder = "Output",
            PrintConfig = new PrintConfig
            {
                DefaultTempate = state.LegacyConfig.TargetPrint.TemplateFile,
                CssClasses = state.LegacyConfig.TargetPrint.StyleClasses.ToCssClasses(),
                PreRenderCode = true,
                Images = state.LegacyConfig.TargetPrint.ImageOptions.ToImageConfig(),
            },
            StaticWebsiteConfig = new StaticWebsiteConfig
            {
                DefaultTempate = state.LegacyConfig.TargetWeb.TemplateFile,
                CopyToOutput = state.LegacyConfig.TargetWeb.TemplateAssets.Select(asset => asset.Source).Where(asset => !string.IsNullOrEmpty(asset)).ToList(),
                DeployHost = state.LegacyConfig.HostName,
                Images = state.LegacyConfig.TargetWeb.ImageOptions.ToImageConfig(),
                CssClasses = state.LegacyConfig.TargetWeb.StyleClasses.ToCssClasses(),
                OpenLinksOutsideHostOnNewTab = state.LegacyConfig.LinksOutSideOfHostOpenNewTab,
                PreRenderCode = false,
                TocConfiguration = new TableOfContentsConfiguration
                {
                    ChapterContainer = ContainerElement.Section,
                    ContainerElement = ContainerElement.Nav,
                    ItemContainer = ItemContainer.UnorderedList,
                    ContainerClass = "",
                    ContainerId = "toc"
                }
            },
            WordpressConfig = new WordpressConfig
            {
                DefaultTempate = state.LegacyConfig.TargetWordpress.TemplateFile,
                AllowComments = false,
                DeployHost = state.LegacyConfig.TargetWordpress.TemplateOptions["WordpressTargetHost"],
                CssClasses = state.LegacyConfig.TargetWordpress.StyleClasses.ToCssClasses(),
                Images = state.LegacyConfig.TargetWordpress.ImageOptions.ToImageConfig(),
                ItemType = state.LegacyConfig.TargetWordpress.TemplateOptions["WordpressItemType"],
                OpenLinksOutsideHostOnNewTab = false,
                TagCategory = state.LegacyConfig.TargetWordpress.TemplateOptions["WordpressTagCategory"],
                PreRenderCode = false,
            }
        };

        await foler.SerializeAsync(FileNameConstants.ConfigFile, config, writeSchema: true);

        return true;
    }
}