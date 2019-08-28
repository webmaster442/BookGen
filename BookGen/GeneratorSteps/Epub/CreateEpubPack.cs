//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Core.Contracts;
using BookGen.Domain;
using System.IO.Compression;

namespace BookGen.GeneratorSteps.Epub
{
    internal class CreateEpubPack : IGeneratorStep
    {
        public void RunStep(RuntimeSettings settings, ILog log)
        {
            log.Info("Creating epub file from contents...");
            var output = settings.OutputDirectory.Combine("book.epub");
            var input = settings.OutputDirectory.Combine("epubtemp");

            ZipFile.CreateFromDirectory(input.ToString(),
                                        output.ToString(),
                                        CompressionLevel.NoCompression,
                                        false);
        }
    }
}
