//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Lib.Domain.Epub;
using BookGen.Lib.Http;
using BookGen.Lib.Internals;

using Microsoft.Extensions.Logging;

namespace BookGen.Lib.Pipeline.Epub;

internal sealed class CreateImageFiles : PipeLineStep<EpubState>
{
    public CreateImageFiles(EpubState state) : base(state)
    {
    }

    public override async Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger)
    {
        logger.LogInformation("Writing {count} images to epub...", State.ImagesData.Count);

        foreach (KeyValuePair<string, string> image in State.ImagesData)
        {
            logger.LogDebug("Writing {image}...", image.Key);
            await State.EpubFile.AddAsync($"EPUB/content/{image.Key}", Convert.FromBase64String(image.Value));
            State.PackageItems.Add(new PackageItem
            {
                Href = $"content/{image.Key}",
                Id = $"id-{IdGenerator.Generate32BitDeterministicId(image.Key)}",
                Mediatype = MimeTypes.GetMimeTypeForFile(image.Key),
            });
        }

        return StepResult.Success;
    }
}
