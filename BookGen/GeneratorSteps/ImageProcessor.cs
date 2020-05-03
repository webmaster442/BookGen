﻿//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Contracts;
using BookGen.Core;
using BookGen.Domain;
using BookGen.Utilities;
using SkiaSharp;
using System;
using System.IO;

namespace BookGen.GeneratorSteps
{
    internal class ImageProcessor : IGeneratorStep
    {
        public void RunStep(RuntimeSettings settings, ILog log)
        {
            if (FsPath.IsEmptyPath(settings.ImageDirectory))
            {
                log.Warning("Images directory is empty string. Skipping image copy & inline step");
                return;
            }

            var targetdir = settings.OutputDirectory.Combine(settings.ImageDirectory.Filename);

            foreach (FsPath file in settings.ImageDirectory.GetAllFiles())
            {
                ProcessImage(file, settings, targetdir, log);
            }

        }

        private void ProcessImage(FsPath file, RuntimeSettings settings, FsPath targetdir, ILog log)
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
                using (var data = ImageUtils.SvgToPng(file, options.MaxWidth, options.MaxHeight))
                {
                    InlineOrSave(file, targetdir, settings, data);
                    return;
                }
            }

            log.Detail("Processing image: {0}", file);
            using (SKBitmap image = ImageUtils.LoadImage(file))
            {
                var format = ImageUtils.GetSkiaImageFormat(file);
                using (SKBitmap resized = ImageUtils.ResizeIfBigger(image, options.MaxWidth, options.MaxHeight))
                {
                    if (options.RecodeJpegToWebp && format == SKEncodedImageFormat.Jpeg)
                    {
                        using (SKData webp = ImageUtils.EncodeWebp(resized, options.WebPQuality))
                        {
                            InlineOrSave(file, targetdir, settings, webp, ".webp");
                        }
                    }
                    else
                    {
                        using (SKData data = ImageUtils.EncodeToFormat(resized, format))
                        {
                            InlineOrSave(file, targetdir, settings, data);
                        }
                    }
                }
            }
        }

        private void InlineOrSave(FsPath file, FsPath targetdir, RuntimeSettings settings, SKData data, string? extensionOverride = null)
        {
            if (data.Size < settings.CurrentBuildConfig.ImageOptions.InlineImageSizeLimit)
            {
                InlineImage(file, settings, data, extensionOverride);
            }
            else
            {
                SaveImage(file, targetdir, data, extensionOverride);
            }
        }

        private void InlineImage(FsPath file, RuntimeSettings settings, SKData data, string? extensionOverride)
        {
            byte[] contents = data.ToArray();

            if (extensionOverride == null)
            {
                string mime = Framework.Server.MimeTypes.GetMimeForExtension(file.Extension);
                string inlinekey = file.ToString().Replace(settings.SourceDirectory.ToString(), settings.OutputDirectory.ToString());
                settings.InlineImgCache.Add(inlinekey, $"data:{mime};base64,{Convert.ToBase64String(contents)}");
            }
            else
            {
                var newFile = Path.ChangeExtension(file.ToString(), extensionOverride);
                var changed = new FsPath(newFile);
                string mime = Framework.Server.MimeTypes.GetMimeForExtension(extensionOverride);
                string inlinekey = changed.ToString().Replace(settings.SourceDirectory.ToString(), settings.OutputDirectory.ToString());
                settings.InlineImgCache.Add(inlinekey, $"data:{mime};base64,{Convert.ToBase64String(contents)}");
            }
        }

        private void SaveImage(FsPath file, FsPath targetdir, SKData data, string? extensionOverride)
        {
            string target;
            if (extensionOverride == null)
            {
                target = targetdir.Combine(file.Filename).ToString();
            }
            else
            {
                target = Path.ChangeExtension(file.ToString(), extensionOverride);
            }
            using (var stream = File.Create(target))
            {
                data.SaveTo(stream);
            }
        }
    }
}