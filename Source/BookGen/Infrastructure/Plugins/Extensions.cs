//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using System.Text.Json;

using Microsoft.Extensions.Logging;

namespace BookGen.Infrastructure.Plugins;

internal static class Extensions
{
    public static bool TryGetPluginManifest(this ZipArchive archive, string packagePath, ILogger logger, [NotNullWhen(true)] out PackageManifest? manifest)
    {
        ZipArchiveEntry? manifestEntry = archive.GetEntry("manifest.json");
        if (manifestEntry == null)
        {
            logger.LogError("Plugin package does not contain a manifest.json: {PackagePath}", packagePath);
            manifest = null;
            return false;
        }
        using Stream manifestStream = manifestEntry.Open();
        try
        {
            PackageManifest? manifestObject = JsonSerializer.Deserialize<PackageManifest>(manifestStream, JsonSerializerOptions.Web);
            if (manifestObject == null)
            {
                logger.LogError("Failed to deserialize manifest.json in plugin package: {PackagePath}", packagePath);
                manifest = null;
                return false;
            }
            IEnumerable<ValidationResult> validationResults = manifestObject.Validate(new ValidationContext(manifestObject));
            if (validationResults.Any())
            {
                foreach (ValidationResult validationResult in validationResults)
                {
                    logger.LogError("Manifest validation error: {ErrorMessage}", validationResult.ErrorMessage);
                }
                manifest = null;
                return false;
            }

            manifest = manifestObject;
            return true;
        }
        catch (Exception)
        {
            logger.LogError("Failed to deserialize manifest.json in plugin package: {PackagePath}", packagePath);
            manifest = null;
            return false;
        }
    }
}
