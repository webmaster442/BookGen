﻿using BookGen.Update.Infrastructure;
using System.IO.Compression;

namespace BookGen.Update.Steps
{
    internal sealed class ExtractZipPackage : IUpdateStepSync
    {
        public bool Execute(GlobalState state)
        {
            if (!File.Exists(state.TempFile))
            {
                state.Issues.Add("Downloaded update file not found");
                return false;
            }

            using (var zip = ZipFile.OpenRead(state.TempFile))
            {
                foreach (var entry in zip.Entries)
                {
                    ExtractRelativeToDirectory(entry, state.TargetDir, true);
                }
            }

            return true;
        }

        internal static string SanitizeEntryFilePath(string entryPath) => entryPath.Replace('\0', '_');

        internal static void ExtractRelativeToDirectory(ZipArchiveEntry source, string destinationDirectoryName, bool overwrite)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(destinationDirectoryName);

            // Note that this will give us a good DirectoryInfo even if destinationDirectoryName exists:
            DirectoryInfo di = Directory.CreateDirectory(destinationDirectoryName);
            string destinationDirectoryFullPath = di.FullName;
            if (!destinationDirectoryFullPath.EndsWith(Path.DirectorySeparatorChar))
                destinationDirectoryFullPath += Path.DirectorySeparatorChar;

            string fileDestinationPath = Path.GetFullPath(Path.Combine(destinationDirectoryFullPath, SanitizeEntryFilePath(source.FullName)));

            if (!fileDestinationPath.StartsWith(destinationDirectoryFullPath, StringComparison.OrdinalIgnoreCase))
                throw new IOException("Extracting Zip entry would have resulted in a file outside the specified destination directory.");

            if (Path.GetFileName(fileDestinationPath).Length == 0)
            {
                // If it is a directory:

                if (source.Length != 0)
                    throw new IOException("Zip entry name ends in directory separator character but contains data.");

                Directory.CreateDirectory(fileDestinationPath);
            }
            else
            {
                // If it is a file:
                // Create containing directory:
                Directory.CreateDirectory(Path.GetDirectoryName(fileDestinationPath)!);
                source.ExtractToFile(fileDestinationPath, overwrite: overwrite);
            }
        }
    }
}
