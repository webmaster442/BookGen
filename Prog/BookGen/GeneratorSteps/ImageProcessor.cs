//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.DomainServices;
using BookGen.Interfaces;
using SkiaSharp;
using System.IO;

namespace BookGen.GeneratorSteps
{
    internal class ImageProcessor : IGeneratorStep
    {
        public void RunStep(IReadonlyRuntimeSettings settings, ILog log)
        {
            log.Info("Processing images...");
            if (FsPath.IsEmptyPath(settings.ImageDirectory))
            {
                log.Warning("Images directory is empty string. Skipping image copy & inline step");
                return;
            }

            var targetdir = settings.OutputDirectory.Combine(settings.ImageDirectory.Filename);

            Parallel.ForEach(settings.ImageDirectory.GetAllFiles(), file =>
            {
                ProcessImage(file, settings, targetdir, log);
            });

        }

        private void ProcessImage(FsPath file, IReadonlyRuntimeSettings settings, FsPath targetdir, ILog log)
        {
            var options = settings.CurrentBuildConfig.ImageOptions;

            if (!ImageUtils.IsImage(file))
            {
                log.Info("Unknown image format, skipping: {0}", file);
                return;
            }

            if (ImageUtils.IsSvg(file))
            {
                log.Detail("Rendering SVG: {0}", file);
                SKEncodedImageFormat format = SKEncodedImageFormat.Png;

                if (options.EncodeSvgAsWebp)
                    format = SKEncodedImageFormat.Webp;

                using (var data = ImageUtils.EncodeSvg(file, options.MaxWidth, options.MaxHeight, format))
                {
                    InlineOrSave(file, targetdir, log, settings, data, ".png");
                    return;
                }
            }

            log.Detail("Processing image: {0}", file);
            using (SKBitmap image = ImageUtils.LoadImage(file))
            {
                var format = ImageUtils.GetSkiaImageFormat(file);
                using (SKBitmap resized = ImageUtils.ResizeIfBigger(image, options.MaxWidth, options.MaxHeight))
                {
                    if ((format == SKEncodedImageFormat.Jpeg && options.RecodeJpegToWebp)
                        || (format == SKEncodedImageFormat.Png && options.RecodePngToWebp))
                    {
                        using SKData webp = ImageUtils.EncodeToFormat(resized,
                                                                      SKEncodedImageFormat.Webp,
                                                                      options.ImageQuality);

                        InlineOrSave(file, targetdir, log, settings, webp, ".webp");
                    }
                    else if (format == SKEncodedImageFormat.Jpeg)
                    {
                        using SKData data = ImageUtils.EncodeToFormat(resized, format, options.ImageQuality);
                        InlineOrSave(file, targetdir, log, settings, data);
                    }
                    else
                    {
                        using SKData data = ImageUtils.EncodeToFormat(resized, format);
                        InlineOrSave(file, targetdir, log, settings, data);
                    }
                }
            }
        }

        private void InlineOrSave(FsPath file, FsPath targetdir, ILog log, IReadonlyRuntimeSettings settings, SKData data, string? extensionOverride = null)
        {
            if (data.Size < settings.CurrentBuildConfig.ImageOptions.InlineImageSizeLimit)
            {
                log.Detail("Inlining: {0}", file);
                InlineImage(file, settings, data, extensionOverride);
            }
            else
            {
                SaveImage(file, targetdir, log, data, extensionOverride);
            }
        }

        private void InlineImage(FsPath file, IReadonlyRuntimeSettings settings, SKData data, string? extensionOverride)
        {
            string base64 = Convert.ToBase64String(data.ToArray());
            string mime = Webmaster442.HttpServerFramework.MimeTypes.GetMimeForExtension(file.Extension);
            string fnmame = file.ToString();

            if (extensionOverride != null)
            {
                mime = Webmaster442.HttpServerFramework.MimeTypes.GetMimeForExtension(extensionOverride);
                fnmame = Path.ChangeExtension(file.ToString(), extensionOverride);
            }

            string key = fnmame.Replace(settings.SourceDirectory.ToString(), settings.OutputDirectory.ToString());
            settings.InlineImgCache.TryAdd(key, $"data:{mime};base64,{base64}");
        }

        private void SaveImage(FsPath file, FsPath targetdir, ILog log, SKData data, string? extensionOverride)
        {
            FsPath target = targetdir.Combine(file.Filename);
            if (extensionOverride != null)
            {
                var newname = Path.ChangeExtension(file.Filename, extensionOverride);
                target = targetdir.Combine(newname);
            }
            using (var stream = target.CreateStream(log))
            {
                log.Detail("Saving image: {0}", target);
                data.SaveTo(stream);
            }
        }
    }
}
