﻿//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Contracts;
using BookGen.Core;
using BookGen.Domain;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace BookGen.GeneratorSteps.Epub
{
    internal class CreateEpubPack : IGeneratorStep
    {
        public void RunStep(RuntimeSettings settings, ILog log)
        {
            log.Info("Creating epub file from contents...");
            FsPath output = settings.OutputDirectory.Combine("book.epub");
            FsPath input = settings.OutputDirectory.Combine("epubtemp");

            output.WriteFile(log, "");

            string[] files = input.GetAllFiles().Select(f => f.ToString()).ToArray();

            int removeLength = input.ToString().Length + 1;

            using (var fs = File.Create(output.ToString()))
            {
                using (var zip = new ZipArchive(fs, ZipArchiveMode.Create))
                {
                    //note: 1st entry is mimetype. It must not be compressed for correct epub export
                    zip.CreateEntryFromFile(files[0], GetEntryName(files[0], removeLength), CompressionLevel.NoCompression);
                    for (int i=1; i<files.Length; i++)
                    {
                        zip.CreateEntryFromFile(files[i], GetEntryName(files[i], removeLength), CompressionLevel.Optimal);
                    }
                }
            }
        }

        private string GetEntryName(string fileName, int rootFolderstringLength)
        {
            return fileName.Substring(rootFolderstringLength).Replace("\\", "/");
        }
    }
}
