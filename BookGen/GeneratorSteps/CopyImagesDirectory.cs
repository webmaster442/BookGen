//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Core;
using BookGen.Core.Contracts;
using BookGen.Domain;
using System;
using System.IO;

namespace BookGen.GeneratorSteps
{
    internal class CopyImagesDirectory : IGeneratorStep
    {
        private readonly bool _inlineEnabled;

        public CopyImagesDirectory(bool inlineEnabled)
        {
            _inlineEnabled = inlineEnabled;
        }

        public void RunStep(GeneratorSettings settings, ILog log)
        {
            var targetdir = settings.OutputDirectory.Combine(settings.ImageDirectory.GetName());
            if (settings.Configruation.InlineImageSizeLimit < 0 
                || !_inlineEnabled)
            {
                log.Info("Copy images to output...");
                settings.ImageDirectory.CopyDirectory(targetdir, log);
                targetdir.ProtectDirectory();
            }
            else
            {
                log.Info("Preparing for Image inlineing...");
                foreach (var file in settings.ImageDirectory.GetAllFiles())
                {
                    FileInfo fi = new FileInfo(file);
                    if (fi.Length < settings.Configruation.InlineImageSizeLimit
                        && (fi.Extension == ".jpg"
                         || fi.Extension == ".png"
                         || fi.Extension == ".jpeg"
                         || fi.Extension == ".webp"))
                    {
                        log.Detail("Inlining image: {0}", fi.FullName);
                        InlineImg(fi.FullName, fi.Extension, settings);
                    }
                    else
                    {

                        CopyImg(fi.FullName, fi.Name, targetdir);
                    }
                }
            }
        }

        private void CopyImg(string Fullname, string name, FsPath targetdir)
        {
            if (!targetdir.IsExisting)
                Directory.CreateDirectory(targetdir.ToString());

            FsPath target = targetdir.Combine(name);

            File.Copy(Fullname, target.ToString());
        }

        private void InlineImg(string fullName, string extension, GeneratorSettings settings)
        {
            byte[] contents = File.ReadAllBytes(fullName);
            string mime = Framework.Server.MimeTypes.GetMimeForExtension(extension);
            settings.InlineImgs.Add(fullName, $"data:{mime};base64,{Convert.ToBase64String(contents)}");
        }
    }
}
