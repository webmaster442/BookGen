//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.IO.Compression;

using Bookgen.Lib.Domain.Epub;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.Epub;

internal sealed class CreateFontFiles : PipeLineStep<EpubState>
{
    public CreateFontFiles(EpubState state) : base(state)
    {
    }

    public override async Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger)
    {
        logger.LogInformation("Creating EPUB font files...");
        await State.EpubFile.AddAsync("JetBrainsMono-Regular.ttf", environment.GetBinaryAssetStream("JetBrainsMono-Regular.ttf"), CompressionLevel.Optimal);
        await State.EpubFile.AddAsync("OpenSans-Regular.ttf", environment.GetBinaryAssetStream("OpenSans-Regular.ttf"), CompressionLevel.Optimal);
        await State.EpubFile.AddAsync("Nunito-Bold.ttf", environment.GetBinaryAssetStream("Nunito-Bold.ttf"), CompressionLevel.Optimal);

        State.PackageItems.AddRange([
            new PackageItem
            {
                Id = "font.jetbrainsmono.regular",
                Href = "JetBrainsMono-Regular.ttf",
                Mediatype = "font/ttf"
            },
            new PackageItem
            {
                Id = "font.opensans.regular",
                Href = "OpenSans-Regular.ttf",
                Mediatype = "font/ttf"
            },
            new PackageItem
            {
                Id = "font.nunito.bold",
                Href = "Nunito-Bold.ttf",
                Mediatype = "font/ttf"
            }
        ]);
        return StepResult.Success;
    }
}
